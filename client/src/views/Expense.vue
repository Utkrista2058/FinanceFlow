<!-- <template>
  <div class="expenses-container">
    
    <div class="page-header">
      <div>
        <h1 class="page-title">
          <i class="bi bi-receipt-cutoff"></i>
          Expenses Tracker
        </h1>
        <p class="page-subtitle">Track and manage all your expenses in one place</p>
      </div>
      <div class="header-actions">
        <div class="stats-mini">
          <div class="stat-mini">
            <span class="stat-mini-label">Total Expenses</span>
            <span class="stat-mini-value">NPR {{ totalExpenses }}</span>
          </div>
          <div class="stat-mini">
            <span class="stat-mini-label">This Month</span>
            <span class="stat-mini-value">{{ expenses.length }}</span>
          </div>
        </div>
        <button class="btn-add-expense" @click="showAddForm = !showAddForm">
          <i :class="showAddForm ? 'bi bi-x-lg' : 'bi bi-plus-lg'"></i>
          <span>{{ showAddForm ? 'Cancel' : 'Add Expense' }}</span>
        </button>
      </div>
    </div>

    
    <transition name="slide-fade">
      <div v-if="showAddForm" class="add-expense-card">
        <div class="form-header">
          <div class="form-icon">
            <i class="bi bi-plus-circle-fill"></i>
          </div>
          <div>
            <h5 class="form-title">Create New Expense</h5>
            <p class="form-subtitle">Fill in the details to record a new expense</p>
          </div>
        </div>
        
        <div class="expense-form">
          <div class="form-row">
            <div class="form-group">
              <label class="form-label">
                <i class="bi bi-calendar-event"></i>
                Date
              </label>
              <input 
                type="date" 
                v-model="newExpense.date" 
                class="form-input"
              />
            </div>

            <div class="form-group">
              <label class="form-label">
                <i class="bi bi-cash-stack"></i>
                Amount
              </label>
              <div class="input-with-prefix">
                <span class="input-prefix">NPR</span>
                <input 
                  type="number" 
                  v-model="newExpense.amount" 
                  placeholder="0.00" 
                  class="form-input with-prefix"
                />
              </div>
            </div>

            <div class="form-group">
              <label class="form-label">
                <i class="bi bi-pencil-square"></i>
                Description
              </label>
              <input 
                type="text" 
                v-model="newExpense.source" 
                placeholder="What was this for?" 
                class="form-input"
              />
            </div>

            <div class="form-group">
              <label class="form-label">
                <i class="bi bi-tag"></i>
                Category
              </label>
              <select v-model="newExpense.categoryId" class="form-select">
                <option disabled value="">Select a category</option>
                <option v-for="cat in categories" :key="cat.id" :value="cat.id">
                  {{ cat.name }}
                </option>
              </select>
            </div>

            <div class="form-group form-actions">
              <button class="btn-save-expense" @click="addExpense">
                <i class="bi bi-check-circle-fill"></i>
                <span>Save Expense</span>
              </button>
            </div>
          </div>
        </div>
      </div>
    </transition>

    
    <div class="expenses-list-card">
      <div class="list-header">
        <h5 class="list-title">
          <i class="bi bi-list-check"></i>
          All Expenses
        </h5>
        <div class="list-controls">
          <div class="filter-dropdown">
            <i class="bi bi-funnel"></i>
            <select class="filter-select">
              <option>All Categories</option>
              <option v-for="cat in categories" :key="cat.id">{{ cat.name }}</option>
            </select>
          </div>
          <div class="search-box">
            <i class="bi bi-search"></i>
            <input type="text" placeholder="Search expenses..." />
          </div>
        </div>
      </div>

      <div v-if="expenses.length > 0" class="expenses-table-wrapper">
        <table class="expenses-table">
          <thead>
            <tr>
              <th class="col-number">#</th>
              <th class="col-date">Date</th>
              <th class="col-amount">Amount</th>
              <th class="col-description">Description</th>
              <th class="col-category">Category</th>
              <th class="col-actions">Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr 
              v-for="(expense, index) in expenses" 
              :key="expense.id"
              class="expense-row"
              :class="{ 'editing-row': expense.isEditing }"
            >
              <td class="col-number">
                <span class="row-number">{{ index + 1 }}</span>
              </td>
              
              <td class="col-date">
                <div v-if="!expense.isEditing" class="date-display">
                  <i class="bi bi-calendar3"></i>
                  {{ formatDate(expense.date) }}
                </div>
                <input 
                  v-else
                  type="date" 
                  v-model="expense.date" 
                  class="form-input-inline"
                />
              </td>

              <td class="col-amount">
                <div v-if="!expense.isEditing" class="amount-display">
                  <span class="currency">NPR</span>
                  <span class="amount">{{ formatAmount(expense.amount) }}</span>
                </div>
                <input 
                  v-else
                  type="number" 
                  v-model="expense.amount" 
                  class="form-input-inline"
                />
              </td>

              <td class="col-description">
                <div v-if="!expense.isEditing" class="description-display">
                  <i :class="getExpenseIcon(expense.source)"></i>
                  <span>{{ expense.source || 'No description' }}</span>
                </div>
                <input 
                  v-else
                  v-model="expense.source" 
                  class="form-input-inline"
                  placeholder="Description"
                />
              </td>

              <td class="col-category">
                <span v-if="!expense.isEditing" class="category-tag">
                  {{ getCategoryName(expense.categoryId) }}
                </span>
                <select 
                  v-else
                  v-model="expense.categoryId" 
                  class="form-select-inline"
                >
                  <option v-for="cat in categories" :key="cat.id" :value="cat.id">
                    {{ cat.name }}
                  </option>
                </select>
              </td>

              <td class="col-actions">
                <div class="action-buttons">
                  <template v-if="!expense.isEditing">
                    <button 
                      class="btn-action btn-action-edit" 
                      @click="editExpense(expense)"
                      title="Edit expense"
                    >
                      <i class="bi bi-pencil-fill"></i>
                    </button>
                    <button 
                      class="btn-action btn-action-delete" 
                      @click="deleteExpense(expense.id)"
                      title="Delete expense"
                    >
                      <i class="bi bi-trash-fill"></i>
                    </button>
                  </template>
                  <template v-else>
                    <button 
                      class="btn-action btn-action-save" 
                      @click="updateExpense(expense)"
                      title="Save changes"
                    >
                      <i class="bi bi-check-lg"></i>
                    </button>
                    <button 
                      class="btn-action btn-action-cancel" 
                      @click="cancelEdit(expense)"
                      title="Cancel"
                    >
                      <i class="bi bi-x-lg"></i>
                    </button>
                  </template>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div v-else class="empty-state">
        <div class="empty-icon">
          <i class="bi bi-receipt"></i>
        </div>
        <h3 class="empty-title">No Expenses Yet</h3>
        <p class="empty-text">Start tracking your expenses by adding your first expense above</p>
        <button class="btn-create-first" @click="showAddForm = true">
          <i class="bi bi-plus-circle"></i>
          Add Your First Expense
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import expenseService from '../services/expenseService';
import categoryService from '../services/categoryService';

export default {
  name: 'Expense',
  data() {
    return {
      expenses: [],
      categories: [],
      showAddForm: false,
      newExpense: { date: '', amount: '', source: '', categoryId: '' },
    };
  },
  computed: {
    totalExpenses() {
      const total = this.expenses.reduce((sum, e) => sum + parseFloat(e.amount || 0), 0);
      return this.formatAmount(total);
    }
  },
  mounted() {
    this.fetchExpenses();
    this.fetchCategories();
  },
  methods: {
    async fetchExpenses() {
      try {
        const res = await expenseService.getExpenses();
        this.expenses = res.data.map(e => ({
          ...e,
          date: e.date ? e.date.split('T')[0] : '',
          isEditing: false,
          source: e.description
        }));
      } catch (err) {
        console.error(err);
        this.showNotification('Failed to fetch expenses', 'error');
      }
    },
    async fetchCategories() {
      try {
        const res = await categoryService.getCategories();
        this.categories = res.data;
      } catch (err) {
        console.error(err);
      }
    },
    editExpense(expense) {
      this.expenses.forEach(e => e.isEditing = false);
      expense.isEditing = true;
    },
    cancelEdit(expense) {
      expense.isEditing = false;
      this.fetchExpenses();
    },
    async updateExpense(expense) {
      if (!expense.date || !expense.amount || !expense.categoryId) {
        this.showNotification('Please fill all required fields', 'warning');
        return;
      }

      try {
        const updated = {
          Id: expense.id,
          Date: new Date(expense.date).toISOString().split('T')[0],
          Amount: parseFloat(expense.amount),
          Description: expense.source,
          CategoryId: parseInt(expense.categoryId)
        };

        await expenseService.updateExpense(expense.id, updated);
        expense.isEditing = false;
        this.fetchExpenses();
        this.showNotification('Expense updated successfully', 'success');
      } catch (err) {
        console.error(err);
        this.showNotification('Failed to update expense', 'error');
      }
    },
    async deleteExpense(id) {
      if (!confirm('Are you sure you want to delete this expense?')) return;
      try {
        await expenseService.deleteExpense(id);
        this.fetchExpenses();
        this.showNotification('Expense deleted successfully', 'success');
      } catch (err) {
        console.error(err);
        this.showNotification('Failed to delete expense', 'error');
      }
    },
    async addExpense() {
      if (!this.newExpense.date || !this.newExpense.amount || !this.newExpense.categoryId) {
        this.showNotification('Please fill all required fields', 'warning');
        return;
      }

      const expenseObject = {
        Date: new Date(this.newExpense.date).toISOString().split('T')[0],
        Amount: parseFloat(this.newExpense.amount),
        Description: this.newExpense.source,
        CategoryId: parseInt(this.newExpense.categoryId)
      };

      try {
        await expenseService.createExpense(expenseObject);
        this.newExpense = { date: '', amount: '', source: '', categoryId: '' };
        this.showAddForm = false;
        this.fetchExpenses();
        this.showNotification('Expense added successfully', 'success');
      } catch (error) {
        console.error("Error adding expense:", error);
        this.showNotification('Failed to add expense', 'error');
      }
    },
    formatAmount(amount) {
      return parseFloat(amount || 0).toLocaleString('en-IN', { 
        minimumFractionDigits: 2, 
        maximumFractionDigits: 2 
      });
    },
    formatDate(dateString) {
      if (!dateString) return '';
      const date = new Date(dateString);
      const options = { month: 'short', day: 'numeric', year: 'numeric' };
      return date.toLocaleDateString('en-US', options);
    },
    getCategoryName(categoryId) {
      const cat = this.categories.find(c => c.id === categoryId);
      return cat ? cat.name : 'Unknown';
    },
    getExpenseIcon(description) {
      if (!description) return 'bi bi-receipt';
      const lower = description.toLowerCase();
      if (lower.includes('food') || lower.includes('restaurant')) return 'bi bi-cup-hot';
      if (lower.includes('transport') || lower.includes('taxi')) return 'bi bi-car-front';
      if (lower.includes('shopping') || lower.includes('store')) return 'bi bi-bag';
      if (lower.includes('entertainment') || lower.includes('movie')) return 'bi bi-film';
      if (lower.includes('bill') || lower.includes('utility')) return 'bi bi-receipt';
      return 'bi bi-receipt';
    },
    showNotification(message, type) {
      alert(message);
    }
  }
};
</script>

<style scoped>
@import url('https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.0/font/bootstrap-icons.css');

.expenses-container {
  padding: 2rem;
  background: linear-gradient(135deg, #f5f7fa 0%, #f0f3f7 100%);
  min-height: 100vh;
}

/* Page Header */
.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
  flex-wrap: wrap;
  gap: 1rem;
}

.page-title {
  font-size: 2rem;
  font-weight: 700;
  color: #1f2937;
  margin: 0;
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.page-subtitle {
  color: #6b7280;
  margin: 0.25rem 0 0 0;
  font-size: 0.95rem;
}

.header-actions {
  display: flex;
  gap: 1.5rem;
  align-items: center;
  flex-wrap: wrap;
}

.stats-mini {
  display: flex;
  gap: 1rem;
}

.stat-mini {
  background: white;
  padding: 0.75rem 1.25rem;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
}

.stat-mini-label {
  display: block;
  font-size: 0.75rem;
  color: #6b7280;
  margin-bottom: 0.25rem;
}

.stat-mini-value {
  display: block;
  font-size: 1.125rem;
  font-weight: 700;
  color: #1f2937;
}

.btn-add-expense {
  padding: 0.875rem 2rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  border-radius: 12px;
  font-weight: 600;
  font-size: 1rem;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  transition: all 0.3s ease;
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.3);
}

.btn-add-expense:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(102, 126, 234, 0.4);
}

/* Add Expense Form */
.slide-fade-enter-active {
  transition: all 0.3s ease;
}

.slide-fade-leave-active {
  transition: all 0.2s ease;
}

.slide-fade-enter-from {
  transform: translateY(-20px);
  opacity: 0;
}

.slide-fade-leave-to {
  transform: translateY(-10px);
  opacity: 0;
}

.add-expense-card {
  background: white;
  border-radius: 16px;
  padding: 2rem;
  margin-bottom: 2rem;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
  border: 2px solid #667eea;
}

.form-header {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-bottom: 2rem;
}

.form-icon {
  width: 60px;
  height: 60px;
  border-radius: 16px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.75rem;
  color: white;
}

.form-title {
  font-size: 1.25rem;
  font-weight: 600;
  color: #1f2937;
  margin: 0;
}

.form-subtitle {
  font-size: 0.875rem;
  color: #6b7280;
  margin: 0.25rem 0 0 0;
}

.expense-form {
  background: #f9fafb;
  padding: 1.5rem;
  border-radius: 12px;
}

.form-row {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1.25rem;
}

.form-group {
  display: flex;
  flex-direction: column;
}

.form-label {
  font-size: 0.875rem;
  font-weight: 600;
  color: #4b5563;
  margin-bottom: 0.5rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.form-input,
.form-select {
  padding: 0.75rem 1rem;
  border: 2px solid #e5e7eb;
  border-radius: 10px;
  font-size: 0.9375rem;
  transition: all 0.3s ease;
  background: white;
}

.form-input:focus,
.form-select:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 4px rgba(102, 126, 234, 0.1);
}

.input-with-prefix {
  position: relative;
}

.input-prefix {
  position: absolute;
  left: 1rem;
  top: 50%;
  transform: translateY(-50%);
  font-weight: 600;
  color: #6b7280;
  font-size: 0.875rem;
}

.form-input.with-prefix {
  padding-left: 3rem;
}

.form-actions {
  display: flex;
  align-items: flex-end;
}

.btn-save-expense {
  width: 100%;
  padding: 0.875rem;
  background: linear-gradient(135deg, #10b981 0%, #059669 100%);
  color: white;
  border: none;
  border-radius: 10px;
  font-weight: 600;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  transition: all 0.3s ease;
  box-shadow: 0 4px 12px rgba(16, 185, 129, 0.3);
}

.btn-save-expense:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(16, 185, 129, 0.4);
}


.expenses-list-card {
  background: white;
  border-radius: 16px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
  overflow: hidden;
}

.list-header {
  padding: 1.5rem 2rem;
  border-bottom: 2px solid #f3f4f6;
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 1rem;
}

.list-title {
  font-size: 1.25rem;
  font-weight: 600;
  color: #1f2937;
  margin: 0;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.list-controls {
  display: flex;
  gap: 1rem;
  align-items: center;
}

.filter-dropdown {
  position: relative;
  display: flex;
  align-items: center;
}

.filter-dropdown i {
  position: absolute;
  left: 0.875rem;
  color: #9ca3af;
  pointer-events: none;
}

.filter-select {
  padding: 0.5rem 1rem 0.5rem 2.5rem;
  border: 2px solid #e5e7eb;
  border-radius: 8px;
  font-size: 0.875rem;
  cursor: pointer;
  transition: all 0.3s ease;
  background: white;
}

.filter-select:focus {
  outline: none;
  border-color: #667eea;
}

.search-box {
  position: relative;
}

.search-box i {
  position: absolute;
  left: 1rem;
  top: 50%;
  transform: translateY(-50%);
  color: #9ca3af;
}

.search-box input {
  padding: 0.5rem 1rem 0.5rem 2.5rem;
  border: 2px solid #e5e7eb;
  border-radius: 8px;
  font-size: 0.875rem;
  width: 250px;
  transition: all 0.3s ease;
}

.search-box input:focus {
  outline: none;
  border-color: #667eea;
  width: 300px;
}


.expenses-table-wrapper {
  overflow-x: auto;
}

.expenses-table {
  width: 100%;
  border-collapse: collapse;
}

.expenses-table thead th {
  padding: 1rem 1.5rem;
  text-align: left;
  font-size: 0.75rem;
  font-weight: 700;
  color: #6b7280;
  text-transform: uppercase;
  letter-spacing: 0.05em;
  border-bottom: 2px solid #f3f4f6;
  background: #f9fafb;
}

.expenses-table tbody tr {
  border-bottom: 1px solid #f3f4f6;
  transition: all 0.2s ease;
}

.expenses-table tbody tr:hover {
  background: #f9fafb;
}

.expenses-table tbody tr.editing-row {
  background: #fef3c7;
}

.expenses-table tbody td {
  padding: 1rem 1.5rem;
}

.row-number {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 32px;
  height: 32px;
  background: #f3f4f6;
  border-radius: 8px;
  font-weight: 600;
  color: #6b7280;
  font-size: 0.875rem;
}

.date-display {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: #4b5563;
  font-size: 0.9375rem;
}

.date-display i {
  color: #9ca3af;
}

.amount-display {
  display: flex;
  align-items: baseline;
  gap: 0.375rem;
}

.currency {
  font-size: 0.75rem;
  color: #6b7280;
  font-weight: 500;
}

.amount {
  font-size: 1rem;
  font-weight: 700;
  color: #ef4444;
}

.description-display {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  color: #1f2937;
}

.description-display i {
  width: 36px;
  height: 36px;
  border-radius: 8px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 0.875rem;
}

.category-tag {
  padding: 0.375rem 0.875rem;
  background: linear-gradient(135deg, #e0e7ff 0%, #ddd6fe 100%);
  color: #5b21b6;
  border-radius: 20px;
  font-size: 0.8125rem;
  font-weight: 600;
  display: inline-block;
}

.form-input-inline,
.form-select-inline {
  padding: 0.5rem 0.75rem;
  border: 2px solid #e5e7eb;
  border-radius: 8px;
  font-size: 0.875rem;
  width: 100%;
  transition: all 0.3s ease;
}

.form-input-inline:focus,
.form-select-inline:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.action-buttons {
  display: flex;
  gap: 0.5rem;
  justify-content: flex-end;
}

.btn-action {
  width: 36px;
  height: 36px;
  border-radius: 8px;
  border: none;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: all 0.2s ease;
  font-size: 0.875rem;
}

.btn-action-edit {
  background: #dbeafe;
  color: #1e40af;
}

.btn-action-edit:hover {
  background: #bfdbfe;
  transform: scale(1.1);
}

.btn-action-delete {
  background: #fee2e2;
  color: #991b1b;
}

.btn-action-delete:hover {
  background: #fecaca;
  transform: scale(1.1);
}

.btn-action-save {
  background: #d1fae5;
  color: #065f46;
}

.btn-action-save:hover {
  background: #a7f3d0;
  transform: scale(1.1);
}

.btn-action-cancel {
  background: #f3f4f6;
  color: #6b7280;
}

.btn-action-cancel:hover {
  background: #e5e7eb;
  transform: scale(1.1);
}


.empty-state {
  text-align: center;
  padding: 4rem 2rem;
}

.empty-icon {
  width: 120px;
  height: 120px;
  margin: 0 auto 1.5rem;
  border-radius: 50%;
  background: linear-gradient(135deg, #f3f4f6 0%, #e5e7eb 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 3rem;
  color: #9ca3af;
}

.empty-title {
  font-size: 1.5rem;
  font-weight: 700;
  color: #1f2937;
  margin: 0 0 0.5rem 0;
}

.empty-text {
  color: #6b7280;
  margin: 0 0 2rem 0;
}

.btn-create-first {
  padding: 0.875rem 2rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  border-radius: 12px;
  font-weight: 600;
  cursor: pointer;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  transition: all 0.3s ease;
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.3);
}

.btn-create-first:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(102, 126, 234, 0.4);
}


@media (max-width: 1024px) {
  .form-row {
    grid-template-columns: repeat(2, 1fr);
  }
  
  .form-actions {
    grid-column: span 2;
  }
}

@media (max-width: 768px) {
  .expenses-container {
    padding: 1rem;
  }

  .page-header {
    flex-direction: column;
    align-items: flex-start;
  }

  .header-actions {
    width: 100%;
    flex-direction: column;
  }

  .stats-mini {
    width: 100%;
    justify-content: space-between;
  }

  .btn-add-expense {
    width: 100%;
    justify-content: center;
  }

  .list-header {
    flex-direction: column;
    align-items: flex-start;
  }

  .list-controls {
    width: 100%;
    flex-direction: column;
  }

  .filter-select,
  .search-box input {
    width: 100%;
  }

  .search-box input:focus {
    width: 100%;
  }

  .form-row {
    grid-template-columns: 1fr;
  }

  .form-actions {
    grid-column: span 1;
  }

  .expenses-table {
    font-size: 0.875rem;
  }

  .expenses-table thead th,
  .expenses-table tbody td {
    padding: 0.75rem 0.5rem;
  }

  .col-number {
    display: none;
  }

  .description-display i {
    width: 32px;
    height: 32px;
    font-size: 0.75rem;
  }

  .action-buttons {
    flex-direction: column;
  }
}

@media (max-width: 480px) {
  .page-title {
    font-size: 1.5rem;
  }

  .stat-mini {
    padding: 0.5rem 1rem;
  }

  .stat-mini-value {
    font-size: 1rem;
  }
}
</style> -->






<template>
  <div class="container mt-5">
    
    <!-- Stats Cards -->
    <div class="row mb-4">
      <div class="col-md-4">
        <div class="stat-card">
          <div class="stat-icon">
            <i class="bi bi-receipt-cutoff"></i>
          </div>
          <div class="stat-content">
            <h6 class="stat-label">Total Expenses</h6>
            <h3 class="stat-value">${{ totalExpenses }}</h3>
          </div>
        </div>
      </div>
      <div class="col-md-4">
        <div class="stat-card">
          <div class="stat-icon" style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);">
            <i class="bi bi-calendar-check"></i>
          </div>
          <div class="stat-content">
            <h6 class="stat-label">This Month</h6>
            <h3 class="stat-value">{{ monthlyExpenseCount }}</h3>
          </div>
        </div>
      </div>
      <div class="col-md-4">
        <div class="stat-card">
          <div class="stat-icon" style="background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%);">
            <i class="bi bi-list-check"></i>
          </div>
          <div class="stat-content">
            <h6 class="stat-label">Total Entries</h6>
            <h3 class="stat-value">{{ expenses.length }}</h3>
          </div>
        </div>
      </div>
    </div>

    <!-- Main Card -->
    <div class="card dark-card shadow-lg">
      <div class="card-header dark-header d-flex justify-content-between align-items-center">
        <div class="d-flex align-items-center gap-3">
          <div class="header-icon">
            <i class="bi bi-receipt-cutoff"></i>
          </div>
          <div>
            <h3 class="mb-0">Expense Management</h3>
            <p class="text-muted mb-0 small">Track and manage all your expenses</p>
          </div>
        </div>
        <button class="btn btn-accent d-flex align-items-center gap-2" @click="addNewExpense">
          <i class="bi bi-plus-circle fs-5"></i> Add Expense
        </button>
      </div>

      <div class="card-body p-0">
        <div class="table-responsive">
          <table class="table table-dark table-hover mb-0">
            <thead class="table-dark text-muted">
              <tr>
                <th class="ps-4">#</th>
                <th><i class="bi bi-calendar3 me-2"></i>Date</th>
                <th><i class="bi bi-currency-dollar me-2"></i>Amount</th>
                <th><i class="bi bi-pencil-square me-2"></i>Description</th>
                <th><i class="bi bi-tag me-2"></i>Category</th>
                <th class="text-end pe-4">Actions</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(expense, index) in expenses" :key="expense.id" class="expense-row">
                <td class="ps-4">
                  <span class="row-number">{{ index + 1 }}</span>
                </td>
                <td>
                  <input type="date" class="form-control dark-input"
                    v-model="expense.date"
                    :readonly="!expense.isEditing"
                    :class="{ 'editing': expense.isEditing }"
                  />
                </td>
                <td>
                  <div class="input-group">
                    <span class="input-group-text dark-input-addon" v-if="!expense.isEditing">$</span>
                    <input type="number" class="form-control dark-input"
                      v-model="expense.amount"
                      :readonly="!expense.isEditing"
                      :class="{ 'editing': expense.isEditing }"
                      step="0.01"
                    />
                  </div>
                </td>
                <td>
                  <input type="text" class="form-control dark-input"
                    v-model="expense.source"
                    :readonly="!expense.isEditing"
                    :class="{ 'editing': expense.isEditing }"
                    placeholder="e.g., Grocery, Transport"
                  />
                </td>
                <td>
                  <select 
                    v-model="expense.categoryId" 
                    class="form-control dark-input"
                    :disabled="!expense.isEditing"
                    :class="{ 'editing': expense.isEditing }"
                  >
                    <option v-for="cat in categories" :key="cat.id" :value="cat.id">
                      {{ cat.name }}
                    </option>
                  </select>
                </td>
                <td class="text-end pe-4">
                  <div class="d-flex gap-2 justify-content-end">
                    <button
                      v-if="!expense.isEditing"
                      class="btn btn-primary btn-sm action-btn"
                      @click="editExpense(expense)"
                      title="Edit"
                    >
                      <i class="bi bi-pencil"></i>
                    </button>
                    <button
                      v-else
                      class="btn btn-success btn-sm action-btn"
                      @click="updateExpense(expense)"
                      title="Save"
                    >
                      <i class="bi bi-check-lg"></i>
                    </button>
                    <button
                      class="btn btn-danger btn-sm action-btn"
                      @click="deleteExpense(expense.id)"
                      title="Delete"
                    >
                      <i class="bi bi-trash"></i>
                    </button>
                  </div>
                </td>
              </tr>

              <tr v-if="expenses.length === 0" class="empty-state">
                <td colspan="6" class="text-center py-5">
                  <div class="empty-icon">
                    <i class="bi bi-inbox"></i>
                  </div>
                  <p class="text-muted mb-2">No expense entries yet</p>
                  <p class="text-muted small">Click "Add Expense" to create your first entry</p>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>

    <!-- Enhanced Modal -->
    <transition name="modal">
      <div v-if="showAddModal" class="modal-backdrop" @click.self="showAddModal = false">
        <div class="modal-dialog">
          <div class="modal-content dark-card">
            <div class="modal-header-custom">
              <div class="d-flex align-items-center gap-3">
                <div class="modal-icon">
                  <i class="bi bi-plus-circle"></i>
                </div>
                <div>
                  <h5 class="mb-0">Add New Expense</h5>
                  <p class="text-muted small mb-0">Fill in the details below</p>
                </div>
              </div>
              <button class="btn-close-custom" @click="showAddModal = false">
                <i class="bi bi-x-lg"></i>
              </button>
            </div>
            
            <div class="modal-body-custom">
              <div class="form-group-custom">
                <label class="form-label-custom">
                  <i class="bi bi-calendar3 me-2"></i>Date
                </label>
                <input type="date" class="form-control dark-input" v-model="newExpense.date" />
              </div>
              
              <div class="form-group-custom">
                <label class="form-label-custom">
                  <i class="bi bi-currency-dollar me-2"></i>Amount
                </label>
                <input type="number" class="form-control dark-input" placeholder="0.00" v-model="newExpense.amount" step="0.01" />
              </div>
              
              <div class="form-group-custom">
                <label class="form-label-custom">
                  <i class="bi bi-pencil-square me-2"></i>Description
                </label>
                <input type="text" class="form-control dark-input" placeholder="e.g., Grocery Shopping, Taxi Fare" v-model="newExpense.source" />
              </div>

              <div class="form-group-custom">
                <label class="form-label-custom">
                  <i class="bi bi-tag me-2"></i>Category
                </label>
                <select class="form-control dark-input" v-model="newExpense.categoryId">
                  <option disabled value="">Select a category</option>
                  <option v-for="cat in categories" :key="cat.id" :value="cat.id">
                    {{ cat.name }}
                  </option>
                </select>
              </div>
            </div>
            
            <div class="modal-footer-custom">
              <button class="btn btn-secondary" @click="showAddModal = false">
                <i class="bi bi-x-circle me-2"></i>Cancel
              </button>
              <button class="btn btn-accent" @click="saveNewExpense">
                <i class="bi bi-check-circle me-2"></i>Add Expense
              </button>
            </div>
          </div>
        </div>
      </div>
    </transition>
  </div>
</template>

<script>
import expenseService from '../services/expenseService';
import categoryService from '../services/categoryService';

export default {
  name: 'Expense',
  data() {
    return {
      expenses: [],
      categories: [],
      showAddModal: false,
      newExpense: { date: '', amount: '', source: '', categoryId: '' },
    };
  },
  computed: {
    totalExpenses() {
      const total = this.expenses.reduce((sum, e) => sum + parseFloat(e.amount || 0), 0);
      return total.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    },
    monthlyExpenseCount() {
      const currentMonth = new Date().getMonth();
      const currentYear = new Date().getFullYear();
      return this.expenses.filter(expense => {
        const expenseDate = new Date(expense.date);
        return expenseDate.getMonth() === currentMonth && expenseDate.getFullYear() === currentYear;
      }).length;
    }
  },
  mounted() {
    this.fetchExpenses();
    this.fetchCategories();
  },
  methods: {
    async fetchExpenses() {
      try {
        const res = await expenseService.getExpenses();
        this.expenses = res.data.map(e => ({
          ...e,
          date: e.date ? e.date.split('T')[0] : '',
          isEditing: false,
          source: e.description
        }));
      } catch (err) {
        console.error("Error fetching expenses:", err);
        alert("Failed to fetch expenses");
      }
    },
    async fetchCategories() {
      try {
        const res = await categoryService.getCategories();
        this.categories = res.data;
      } catch (err) {
        console.error("Error fetching categories:", err);
      }
    },
    addNewExpense() {
      this.showAddModal = true;
    },
    async saveNewExpense() {
      if (!this.newExpense.date || !this.newExpense.amount || !this.newExpense.categoryId) {
        alert('Please fill all required fields');
        return;
      }

      const expenseObject = {
        Date: new Date(this.newExpense.date).toISOString().split('T')[0],
        Amount: parseFloat(this.newExpense.amount),
        Description: this.newExpense.source,
        CategoryId: parseInt(this.newExpense.categoryId)
      };

      try {
        await expenseService.createExpense(expenseObject);
        this.showAddModal = false;
        this.newExpense = { date: '', amount: '', source: '', categoryId: '' };
        this.fetchExpenses();
      } catch (error) {
        console.error("Error adding expense:", error);
        alert("Failed to add expense");
      }
    },
    editExpense(expense) {
      this.expenses.forEach(e => e.isEditing = false);
      expense.isEditing = true;
    },
    async updateExpense(expense) {
      if (!expense.date || !expense.amount || !expense.categoryId) {
        alert('Please fill all required fields');
        return;
      }

      try {
        const updated = {
          Id: expense.id,
          Date: new Date(expense.date).toISOString().split('T')[0],
          Amount: parseFloat(expense.amount),
          Description: expense.source,
          CategoryId: parseInt(expense.categoryId)
        };

        await expenseService.updateExpense(expense.id, updated);
        expense.isEditing = false;
        this.fetchExpenses();
      } catch (err) {
        console.error("Error updating expense:", err);
        alert("Failed to update expense");
      }
    },
    async deleteExpense(id) {
      if (!confirm('Are you sure you want to delete this expense?')) return;
      try {
        await expenseService.deleteExpense(id);
        this.fetchExpenses();
      } catch (err) {
        console.error("Error deleting expense:", err);
        alert("Failed to delete expense");
      }
    }
  }
};
</script>

<style scoped>
/* Stats Cards */
.stat-card {
  background: linear-gradient(135deg, #1e1e2f 0%, #27293d 100%);
  border-radius: 16px;
  padding: 1.5rem;
  display: flex;
  align-items: center;
  gap: 1rem;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
  transition: transform 0.3s ease, box-shadow 0.3s ease;
  border: 1px solid rgba(255, 255, 255, 0.05);
}

.stat-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 8px 30px rgba(79, 91, 213, 0.3);
}

.stat-icon {
  width: 60px;
  height: 60px;
  border-radius: 14px;
  background: linear-gradient(135deg, #4f5bd5 0%, #6c7df5 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
  color: white;
  box-shadow: 0 4px 15px rgba(79, 91, 213, 0.4);
}

.stat-content {
  flex: 1;
}

.stat-label {
  color: #8b8fa3;
  font-size: 0.85rem;
  margin-bottom: 0.25rem;
  font-weight: 500;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.stat-value {
  color: #fff;
  font-size: 1.75rem;
  font-weight: 700;
  margin: 0;
}

/* Main Card */
.dark-card {
  background: linear-gradient(135deg, #1e1e2f 0%, #27293d 100%);
  border-radius: 16px;
  overflow: hidden;
  border: 1px solid rgba(255, 255, 255, 0.05);
  box-shadow: 0 10px 40px rgba(0, 0, 0, 0.4);
}

.dark-header {
  background: linear-gradient(135deg, #27293d 0%, #2d3148 100%);
  color: #fff;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
  padding: 1.5rem 2rem;
  position: relative;
  overflow: hidden;
}

.dark-header::before {
  content: '';
  position: absolute;
  top: -50%;
  right: -50%;
  width: 200%;
  height: 200%;
  background: radial-gradient(circle, rgba(79, 91, 213, 0.1) 0%, transparent 70%);
  pointer-events: none;
}

.header-icon {
  width: 50px;
  height: 50px;
  border-radius: 12px;
  background: linear-gradient(135deg, #4f5bd5 0%, #6c7df5 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
  box-shadow: 0 4px 15px rgba(79, 91, 213, 0.4);
}

/* Table Styles */
.table-responsive {
  overflow-x: auto;
}

.table-dark {
  background-color: transparent;
  color: #cfd2e1;
  margin: 0;
}

.table-dark thead th {
  background: linear-gradient(135deg, #2a2c42 0%, #323550 100%);
  border: none;
  padding: 1rem;
  font-weight: 600;
  text-transform: uppercase;
  font-size: 0.75rem;
  letter-spacing: 1px;
  color: #8b8fa3;
}

.expense-row {
  transition: all 0.3s ease;
  border-bottom: 1px solid rgba(255, 255, 255, 0.05);
}

.expense-row:hover {
  background: linear-gradient(90deg, rgba(79, 91, 213, 0.1) 0%, rgba(108, 125, 245, 0.05) 100%);
  transform: scale(1.01);
}

.expense-row td {
  padding: 1rem;
  vertical-align: middle;
}

.row-number {
  display: inline-block;
  width: 30px;
  height: 30px;
  line-height: 30px;
  text-align: center;
  border-radius: 8px;
  background: rgba(79, 91, 213, 0.2);
  color: #6c7df5;
  font-weight: 600;
  font-size: 0.85rem;
}

/* Input Styles */
.dark-input {
  background-color: rgba(44, 47, 74, 0.5);
  color: #fff;
  border: 1px solid rgba(255, 255, 255, 0.05);
  border-radius: 8px;
  padding: 0.5rem 0.75rem;
  transition: all 0.3s ease;
}

.dark-input:focus {
  background-color: rgba(44, 47, 74, 0.8);
  border-color: #4f5bd5;
  box-shadow: 0 0 0 3px rgba(79, 91, 213, 0.2);
  outline: none;
  color: #fff;
}

.dark-input.editing {
  background-color: rgba(79, 91, 213, 0.1);
  border-color: #4f5bd5;
}

.dark-input:read-only,
.dark-input:disabled {
  background-color: transparent;
  border-color: transparent;
}

.dark-input:read-only:hover,
.dark-input:disabled:hover {
  background-color: rgba(44, 47, 74, 0.3);
}

.input-group-text.dark-input-addon {
  background-color: transparent;
  border: none;
  color: #4f5bd5;
  font-weight: 600;
}

/* Buttons */
.btn-accent {
  background: linear-gradient(135deg, #4f5bd5 0%, #6c7df5 100%);
  color: #fff;
  border: none;
  border-radius: 10px;
  padding: 0.65rem 1.5rem;
  font-weight: 600;
  transition: all 0.3s ease;
  box-shadow: 0 4px 15px rgba(79, 91, 213, 0.3);
}

.btn-accent:hover {
  background: linear-gradient(135deg, #6c7df5 0%, #4f5bd5 100%);
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(79, 91, 213, 0.5);
}

.action-btn {
  width: 36px;
  height: 36px;
  padding: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 8px;
  transition: all 0.3s ease;
  border: none;
}

.btn-primary.action-btn {
  background: linear-gradient(135deg, #4f5bd5 0%, #6c7df5 100%);
  box-shadow: 0 2px 8px rgba(79, 91, 213, 0.3);
}

.btn-primary.action-btn:hover {
  transform: scale(1.1);
  box-shadow: 0 4px 12px rgba(79, 91, 213, 0.5);
}

.btn-success.action-btn {
  background: linear-gradient(135deg, #0acf83 0%, #06b770 100%);
  box-shadow: 0 2px 8px rgba(10, 207, 131, 0.3);
}

.btn-success.action-btn:hover {
  transform: scale(1.1);
  box-shadow: 0 4px 12px rgba(10, 207, 131, 0.5);
}

.btn-danger.action-btn {
  background: linear-gradient(135deg, #ff4757 0%, #ff3838 100%);
  box-shadow: 0 2px 8px rgba(255, 71, 87, 0.3);
}

.btn-danger.action-btn:hover {
  transform: scale(1.1);
  box-shadow: 0 4px 12px rgba(255, 71, 87, 0.5);
}

/* Empty State */
.empty-state td {
  background: transparent !important;
}

.empty-icon {
  font-size: 4rem;
  color: #3a3f58;
  margin-bottom: 1rem;
}

/* Modal Styles */
.modal-backdrop {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.7);
  backdrop-filter: blur(5px);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1050;
  animation: fadeIn 0.3s ease;
}

.modal-dialog {
  max-width: 500px;
  width: 90%;
  animation: slideUp 0.3s ease;
}

.modal-content.dark-card {
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.6);
  border: 1px solid rgba(255, 255, 255, 0.1);
}

.modal-header-custom {
  padding: 1.5rem 2rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
  display: flex;
  justify-content: space-between;
  align-items: center;
  background: linear-gradient(135deg, #27293d 0%, #2d3148 100%);
}

.modal-icon {
  width: 50px;
  height: 50px;
  border-radius: 12px;
  background: linear-gradient(135deg, #4f5bd5 0%, #6c7df5 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
  color: white;
  box-shadow: 0 4px 15px rgba(79, 91, 213, 0.4);
}

.btn-close-custom {
  background: transparent;
  border: none;
  color: #8b8fa3;
  font-size: 1.2rem;
  cursor: pointer;
  padding: 0.5rem;
  border-radius: 8px;
  transition: all 0.3s ease;
}

.btn-close-custom:hover {
  background: rgba(255, 255, 255, 0.1);
  color: #fff;
}

.modal-body-custom {
  padding: 2rem;
}

.form-group-custom {
  margin-bottom: 1.5rem;
}

.form-label-custom {
  display: block;
  margin-bottom: 0.5rem;
  color: #cfd2e1;
  font-weight: 500;
  font-size: 0.9rem;
}

.modal-footer-custom {
  padding: 1.5rem 2rem;
  border-top: 1px solid rgba(255, 255, 255, 0.1);
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  background: linear-gradient(135deg, #1e1e2f 0%, #27293d 100%);
}

.btn-secondary {
  background: rgba(255, 255, 255, 0.05);
  color: #cfd2e1;
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 10px;
  padding: 0.65rem 1.5rem;
  font-weight: 600;
  transition: all 0.3s ease;
}

.btn-secondary:hover {
  background: rgba(255, 255, 255, 0.1);
  border-color: rgba(255, 255, 255, 0.2);
  color: #fff;
}

/* Animations */
@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

@keyframes slideUp {
  from {
    transform: translateY(50px);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}

.modal-enter-active, .modal-leave-active {
  transition: opacity 0.3s ease;
}

.modal-enter-from, .modal-leave-to {
  opacity: 0;
}

.modal-enter-active .modal-dialog, .modal-leave-active .modal-dialog {
  transition: transform 0.3s ease;
}

.modal-enter-from .modal-dialog, .modal-leave-to .modal-dialog {
  transform: translateY(50px);
}
</style>
