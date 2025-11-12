<!-- 

<template>
  <div class="container mt-5">
    <div class="card shadow-lg rounded-4 border-0">
      
      <div class="card-header d-flex justify-content-between align-items-center bg-primary text-white py-3 px-4 rounded-top-4">
        <h3 class="mb-0">Categories</h3>
        <div class="d-flex align-items-center gap-2">
          <input
            v-model="newCategoryName"
            class="form-control form-control-lg rounded-pill"
            placeholder="New category name"
          />
          <button class="btn btn-success btn-lg rounded-pill d-flex align-items-center gap-2" @click="addCategory">
            <i class="bi bi-plus-circle "></i> Add
          </button>
        </div>
      </div>

      
      <div class="card-body p-0">
        <table class="table table-hover align-middle mb-0">
          <thead class="table-light">
            <tr>
              <th scope="col">S.N</th>
              <th scope="col">Name</th>
              <th scope="col" class="text-end">Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(category, index) in categories" :key="category.id">
              <th scope="row">{{ index + 1 }}</th>
              <td>
                <input
                  v-model="category.name"
                  class="form-control form-control-lg rounded-pill"
                />
              </td>
             
              <td class="text-end">
  <button
    class="btn btn-primary btn-sm me-1 d-flex align-items-center"
    @click="updateCategory(category)"
    title="Edit"
  >
    <i class="bi bi-pencil-square me-1 fs-6"></i> Edit
  </button>
  <button
    class="btn btn-danger btn-sm d-flex align-items-center"
    @click="deleteCategory(category.id)"
    title="Delete"
  >
    <i class="bi bi-trash-fill me-1 fs-6"></i> Delete
  </button>
</td>

            </tr>
            <tr v-if="categories.length === 0">
              <td colspan="3" class="text-center text-muted py-3">No categories found.</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script>
import categoryService from '../services/categoryService';

export default {
  name: "Category",
  data() {
    return {
      categories: [],
      newCategoryName: ""
    };
  },
  mounted() {
    this.getCategories();
  },
  methods: {
    async getCategories() {
      try {
        const res = await categoryService.getCategories();
        this.categories = res.data;
      } catch (error) {
        console.error("Error fetching categories:", error);
        alert("Failed to fetch categories");
      }
    },
    async addCategory() {
      if (!this.newCategoryName) return alert("Category name required");
      try {
        await categoryService.createCategory({ name: this.newCategoryName });
        this.newCategoryName = "";
        this.getCategories();
      } catch (error) {
        console.error("Error adding category:", error);
        alert("Failed to add category");
      }
    },
    async updateCategory(category) {
      try {
        await categoryService.updateCategory(category.id, { name: category.name });
        this.getCategories();
        alert("Category updated successfully!");
      } catch (error) {
        console.error("Error updating category:", error);
        alert("Failed to update category");
      }
    },
    async deleteCategory(id) {
      if (!confirm("Are you sure you want to delete this category?")) return;
      try {
        await categoryService.deleteCategory(id);
        this.getCategories();
      } catch (error) {
        console.error("Error deleting category:", error);
        alert("Failed to delete category");
      }
    }
  }
};
</script>

<style scoped>
.card {
  background-color: #f8f9fa;
}


input.form-control-lg {
  background-color: #e9ecef;
  border: 1px solid #ced4da;
  transition: all 0.2s;
}

input.form-control-lg:focus {
  outline: none;
  border-color: #4f5bd5;
  box-shadow: 0 0 0 2px rgba(79, 91, 213, 0.25);
}


.btn-lg {
  font-size: 1.2rem;
  padding: 0.4rem 0.5rem;
  transition: all 0.2s;
}

.btn-primary {
  background-color: #0d6efd;
  border-color: #0d6efd;
}

.btn-primary:hover {
  background-color: #0b5ed7;
  border-color: #0a58ca;
}

.btn-danger {
  background-color: #dc3545;
  border-color: #dc3545;
}

.btn-danger:hover {
  background-color: #c82333;
  border-color: #bd2130;
}

.btn-success {
  background-color: #28a745;
  border-color: #28a745;
}

.btn-success:hover {
  background-color: #218838;
  border-color: #1e7e34;
}

.table-hover tbody tr:hover {
  background-color: #f1f3f5;
}
</style>


 -->


 <template>
  <div class="categories-container">
    <!-- Page Header -->
    <div class="page-header">
      <div>
        <h1 class="page-title">
          <i class="bi bi-grid-3x3-gap-fill"></i>
          Categories Management
        </h1>
        <p class="page-subtitle">Organize and manage your expense categories</p>
      </div>
      <div class="header-stats">
        <div class="stat-badge">
          <i class="bi bi-tag-fill"></i>
          <span>{{ categories.length }} Categories</span>
        </div>
      </div>
    </div>

    <!-- Add New Category Section -->
    <div class="add-category-card">
      <div class="add-category-header">
        <div class="add-icon-wrapper">
          <i class="bi bi-plus-circle-fill"></i>
        </div>
        <div>
          <h5 class="add-title">Create New Category</h5>
          <p class="add-subtitle">Add a new category to organize your expenses</p>
        </div>
      </div>
      <div class="add-category-form">
        <div class="input-group-custom">
          <i class="bi bi-tag input-icon"></i>
          <input
            v-model="newCategoryName"
            class="input-custom"
            placeholder="Enter category name (e.g., Food, Transportation)"
            @keyup.enter="addCategory"
          />
        </div>
        <button class="btn-add-category" @click="addCategory">
          <i class="bi bi-plus-lg"></i>
          <span>Add Category</span>
        </button>
      </div>
    </div>

    <!-- Categories List -->
    <div class="categories-list-card">
      <div class="categories-header">
        <h5 class="categories-title">
          <i class="bi bi-list-ul"></i>
          All Categories
        </h5>
        <div class="search-box">
          <i class="bi bi-search"></i>
          <input type="text" placeholder="Search categories..." />
        </div>
      </div>

      <div class="categories-grid" v-if="categories.length > 0">
        <div 
          class="category-card" 
          v-for="(category, index) in categories" 
          :key="category.id"
          :class="{ 'editing-mode': category.isEditing }"
          :style="{ animationDelay: `${index * 0.05}s` }"
        >
          <div class="category-icon" :style="{ background: getGradient(index) }">
            <i :class="getCategoryIcon(category.name)"></i>
          </div>
          
          <div class="category-content">
            <div class="category-number">#{{ index + 1 }}</div>
            
            <!-- Display Mode -->
            <div v-if="!category.isEditing" class="category-name-display">
              {{ category.name }}
            </div>
            
            <!-- Edit Mode -->
            <input
              v-else
              v-model="category.editedName"
              class="category-input-edit"
              @keyup.enter="saveCategory(category)"
              @keyup.esc="cancelEdit(category)"
              ref="editInput"
            />
          </div>

          <div class="category-actions">
            <!-- Edit Mode Actions -->
            <template v-if="category.isEditing">
              <button
                class="btn-action btn-save"
                @click="saveCategory(category)"
                title="Save changes"
              >
                <i class="bi bi-check-lg"></i>
              </button>
              <button
                class="btn-action btn-cancel"
                @click="cancelEdit(category)"
                title="Cancel"
              >
                <i class="bi bi-x-lg"></i>
              </button>
            </template>
            
            <!-- Display Mode Actions -->
            <template v-else>
              <button
                class="btn-action btn-edit"
                @click="startEdit(category)"
                title="Edit category"
              >
                <i class="bi bi-pencil-fill"></i>
              </button>
              <button
                class="btn-action btn-delete"
                @click="deleteCategory(category.id)"
                title="Delete category"
              >
                <i class="bi bi-trash-fill"></i>
              </button>
            </template>
          </div>
        </div>
      </div>

      <div v-else class="empty-state">
        <div class="empty-icon">
          <i class="bi bi-inbox"></i>
        </div>
        <h3 class="empty-title">No Categories Yet</h3>
        <p class="empty-text">Start by creating your first category to organize your expenses</p>
        <button class="btn-create-first" @click="focusInput">
          <i class="bi bi-plus-circle"></i>
          Create Your First Category
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import categoryService from '../services/categoryService';

export default {
  name: "Category",
  data() {
    return {
      categories: [],
      newCategoryName: ""
    };
  },
  mounted() {
    this.getCategories();
  },
  methods: {
    async getCategories() {
      try {
        const res = await categoryService.getCategories();
        this.categories = res.data.map(cat => ({ 
          ...cat, 
          isEditing: false,
          editedName: cat.name,
          originalName: cat.name
        }));
      } catch (error) {
        console.error("Error fetching categories:", error);
        this.showNotification("Failed to fetch categories", "error");
      }
    },
    
    async addCategory() {
      if (!this.newCategoryName.trim()) {
        this.showNotification("Please enter a category name", "warning");
        return;
      }
      try {
        await categoryService.createCategory({ name: this.newCategoryName });
        this.newCategoryName = "";
        this.getCategories();
        this.showNotification("Category added successfully!", "success");
      } catch (error) {
        console.error("Error adding category:", error);
        this.showNotification("Failed to add category", "error");
      }
    },
    
    startEdit(category) {
      // Cancel any other editing categories
      this.categories.forEach(cat => {
        if (cat.id !== category.id && cat.isEditing) {
          this.cancelEdit(cat);
        }
      });
      
      category.editedName = category.name;
      category.isEditing = true;
      
      // Focus the input field after Vue updates the DOM
      this.$nextTick(() => {
        const editInputs = this.$refs.editInput;
        if (editInputs && editInputs.length) {
          const input = Array.isArray(editInputs) ? editInputs[editInputs.length - 1] : editInputs;
          input.focus();
          input.select();
        }
      });
    },
    
    cancelEdit(category) {
      category.isEditing = false;
      category.editedName = category.name;
    },
    
    async saveCategory(category) {
      if (!category.editedName.trim()) {
        this.showNotification("Category name cannot be empty", "warning");
        return;
      }
      
      if (category.editedName === category.name) {
        category.isEditing = false;
        return;
      }
      
      try {
        // Backend expects full category object with Id
        const updatedCategory = {
          id: category.id,
          name: category.editedName
        };
        
        console.log('Updating category:', updatedCategory);
        
        const response = await categoryService.updateCategory(category.id, updatedCategory);
        console.log('Update response:', response);
        
        category.name = category.editedName;
        category.isEditing = false;
        this.showNotification("Category updated successfully!", "success");
      } catch (error) {
        console.error("Error updating category:", error);
        console.error("Error details:", error.response?.data);
        console.error("Error status:", error.response?.status);
        
        const errorMessage = error.response?.data?.message || 
                           error.response?.data?.error || 
                           "Failed to update category";
        this.showNotification(errorMessage, "error");
        category.editedName = category.name;
      }
    },
    
    async deleteCategory(id) {
      if (!confirm("Are you sure you want to delete this category?")) return;
      try {
        await categoryService.deleteCategory(id);
        this.getCategories();
        this.showNotification("Category deleted successfully!", "success");
      } catch (error) {
        console.error("Error deleting category:", error);
        this.showNotification("Failed to delete category", "error");
      }
    },
    
    showNotification(message, type) {
      alert(message);
    },
    
    getGradient(index) {
      const gradients = [
        'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
        'linear-gradient(135deg, #f093fb 0%, #f5576c 100%)',
        'linear-gradient(135deg, #4facfe 0%, #00f2fe 100%)',
        'linear-gradient(135deg, #43e97b 0%, #38f9d7 100%)',
        'linear-gradient(135deg, #fa709a 0%, #fee140 100%)',
        'linear-gradient(135deg, #30cfd0 0%, #330867 100%)',
        'linear-gradient(135deg, #a8edea 0%, #fed6e3 100%)',
        'linear-gradient(135deg, #ff9a9e 0%, #fecfef 100%)',
      ];
      return gradients[index % gradients.length];
    },
    
    getCategoryIcon(name) {
      const lowerName = name.toLowerCase();
      if (lowerName.includes('food') || lowerName.includes('dining')) return 'bi bi-cup-hot-fill';
      if (lowerName.includes('transport') || lowerName.includes('travel')) return 'bi bi-car-front-fill';
      if (lowerName.includes('shopping') || lowerName.includes('retail')) return 'bi bi-bag-fill';
      if (lowerName.includes('entertainment') || lowerName.includes('fun')) return 'bi bi-film';
      if (lowerName.includes('bills') || lowerName.includes('utilities')) return 'bi bi-receipt';
      if (lowerName.includes('health') || lowerName.includes('medical')) return 'bi bi-heart-pulse-fill';
      if (lowerName.includes('education') || lowerName.includes('learning')) return 'bi bi-book-fill';
      if (lowerName.includes('home') || lowerName.includes('house')) return 'bi bi-house-fill';
      if (lowerName.includes('sport') || lowerName.includes('gym')) return 'bi bi-trophy-fill';
      return 'bi bi-tag-fill';
    },
    
    focusInput() {
      const input = document.querySelector('.input-custom');
      if (input) input.focus();
    }
  }
};
</script>

<style scoped>
@import url('https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.0/font/bootstrap-icons.css');

.categories-container {
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

.header-stats {
  display: flex;
  gap: 1rem;
}

.stat-badge {
  background: white;
  padding: 0.75rem 1.25rem;
  border-radius: 50px;
  font-size: 0.875rem;
  font-weight: 600;
  color: #667eea;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

/* Add Category Card */
.add-category-card {
  background: white;
  border-radius: 16px;
  padding: 2rem;
  margin-bottom: 2rem;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
  border: 2px solid transparent;
  transition: all 0.3s ease;
}

.add-category-card:hover {
  border-color: #667eea;
  box-shadow: 0 8px 24px rgba(102, 126, 234, 0.15);
}

.add-category-header {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-bottom: 1.5rem;
}

.add-icon-wrapper {
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

.add-title {
  font-size: 1.25rem;
  font-weight: 600;
  color: #1f2937;
  margin: 0;
}

.add-subtitle {
  font-size: 0.875rem;
  color: #6b7280;
  margin: 0.25rem 0 0 0;
}

.add-category-form {
  display: flex;
  gap: 1rem;
  align-items: center;
}

.input-group-custom {
  flex: 1;
  position: relative;
}

.input-icon {
  position: absolute;
  left: 1.25rem;
  top: 50%;
  transform: translateY(-50%);
  color: #9ca3af;
  font-size: 1.125rem;
}

.input-custom {
  width: 100%;
  padding: 0.875rem 1.25rem 0.875rem 3.25rem;
  border: 2px solid #e5e7eb;
  border-radius: 12px;
  font-size: 1rem;
  transition: all 0.3s ease;
  background: #f9fafb;
}

.input-custom:focus {
  outline: none;
  border-color: #667eea;
  background: white;
  box-shadow: 0 0 0 4px rgba(102, 126, 234, 0.1);
}

.btn-add-category {
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

.btn-add-category:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(102, 126, 234, 0.4);
}

.btn-add-category:active {
  transform: translateY(0);
}

/* Categories List Card */
.categories-list-card {
  background: white;
  border-radius: 16px;
  padding: 2rem;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
}

.categories-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
  padding-bottom: 1rem;
  border-bottom: 2px solid #f3f4f6;
}

.categories-title {
  font-size: 1.25rem;
  font-weight: 600;
  color: #1f2937;
  margin: 0;
  display: flex;
  align-items: center;
  gap: 0.5rem;
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

/* Categories Grid */
.categories-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 1.5rem;
}

@keyframes slideIn {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.category-card {
  background: white;
  border: 2px solid #f3f4f6;
  border-radius: 16px;
  padding: 1.5rem;
  display: flex;
  align-items: center;
  gap: 1rem;
  transition: all 0.3s ease;
  animation: slideIn 0.4s ease forwards;
  position: relative;
  overflow: hidden;
}

.category-card::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 3px;
  background: linear-gradient(90deg, #667eea 0%, #764ba2 100%);
  transform: scaleX(0);
  transition: transform 0.3s ease;
}

.category-card:hover::before {
  transform: scaleX(1);
}

.category-card.editing-mode {
  border-color: #667eea;
  box-shadow: 0 8px 24px rgba(102, 126, 234, 0.15);
  background: #fafbff;
}

.category-card.editing-mode::before {
  transform: scaleX(1);
}

.category-card:not(.editing-mode):hover {
  border-color: #e5e7eb;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
  transform: translateY(-2px);
}

.category-icon {
  width: 56px;
  height: 56px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
  color: white;
  flex-shrink: 0;
}

.category-content {
  flex: 1;
  min-width: 0;
}

.category-number {
  font-size: 0.75rem;
  color: #9ca3af;
  font-weight: 600;
  margin-bottom: 0.25rem;
}

.category-name-display {
  font-size: 1rem;
  font-weight: 600;
  color: #1f2937;
  padding: 0.5rem 0;
  word-break: break-word;
}

.category-input-edit {
  width: 100%;
  padding: 0.5rem 0.75rem;
  border: 2px solid #667eea;
  border-radius: 8px;
  font-size: 1rem;
  font-weight: 600;
  color: #1f2937;
  background: white;
  transition: all 0.3s ease;
}

.category-input-edit:focus {
  outline: none;
  border-color: #764ba2;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.category-actions {
  display: flex;
  gap: 0.5rem;
  opacity: 0;
  transition: opacity 0.3s ease;
}

.category-card:hover .category-actions,
.category-card.editing-mode .category-actions {
  opacity: 1;
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
  font-size: 1rem;
}

.btn-edit {
  background: #dbeafe;
  color: #1e40af;
}

.btn-edit:hover {
  background: #bfdbfe;
  transform: scale(1.1);
}

.btn-save {
  background: #d1fae5;
  color: #065f46;
}

.btn-save:hover {
  background: #a7f3d0;
  transform: scale(1.1);
}

.btn-cancel {
  background: #fee2e2;
  color: #991b1b;
}

.btn-cancel:hover {
  background: #fecaca;
  transform: scale(1.1);
}

.btn-delete {
  background: #fee2e2;
  color: #991b1b;
}

.btn-delete:hover {
  background: #fecaca;
  transform: scale(1.1);
}

/* Empty State */
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

/* Responsive */
@media (max-width: 768px) {
  .categories-container {
    padding: 1rem;
  }

  .page-header {
    flex-direction: column;
    align-items: flex-start;
    gap: 1rem;
  }

  .add-category-form {
    flex-direction: column;
  }

  .btn-add-category {
    width: 100%;
    justify-content: center;
  }

  .categories-header {
    flex-direction: column;
    gap: 1rem;
    align-items: flex-start;
  }

  .search-box input {
    width: 100%;
  }

  .search-box input:focus {
    width: 100%;
  }

  .categories-grid {
    grid-template-columns: 1fr;
  }

  .category-actions {
    opacity: 1;
  }
}
</style>