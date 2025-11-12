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

<!-- <script>
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
        // Date: new Date(this.newExpense.date).toISOString().split('T')[0],
        Date: new Date(expense.date).toISOString().split('T')[0],
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
</script> -->
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

      const payload = {
        date: new Date(this.newExpense.date).toISOString(), // send full UTC ISO string
        amount: parseFloat(this.newExpense.amount),
        description: this.newExpense.source,
        categoryId: parseInt(this.newExpense.categoryId)
      };

      try {
        await expenseService.createExpense(payload);
        this.showAddModal = false;
        this.newExpense = { date: '', amount: '', source: '', categoryId: '' };
        await this.fetchExpenses();
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

      const payload = {
        date: new Date(expense.date).toISOString(), // send full UTC ISO string
        amount: parseFloat(expense.amount),
        description: expense.source,
        categoryId: parseInt(expense.categoryId)
      };

      try {
        await expenseService.updateExpense(expense.id, payload);
        expense.isEditing = false;
        await this.fetchExpenses();
      } catch (err) {
        console.error("Error updating expense:", err);
        alert("Failed to update expense");
      }
    },
    async deleteExpense(id) {
      if (!confirm('Are you sure you want to delete this expense?')) return;
      try {
        await expenseService.deleteExpense(id);
        await this.fetchExpenses();
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
