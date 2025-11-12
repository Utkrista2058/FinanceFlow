<template>
  <div class="dashboard-container">
    
    <div class="dashboard-header">
      <div>
        <h1 class="dashboard-title">Financial Dashboard</h1>
        <p class="dashboard-subtitle">Track your income and expenses at a glance</p>
      </div>
      <div class="header-actions">
        <button @click="downloadExcelReport" class="download-btn" :disabled="isDownloading">
          <i class="bi bi-download"></i>
          <span v-if="!isDownloading">Download Yearly Report</span>
          <span v-else>Downloading...</span>
        </button>
        <span class="date-badge">
          <i class="bi bi-calendar3"></i>
          {{ currentDate }}
        </span>
      </div>
    </div>

    
    <!-- 🌟 SMART INSIGHTS BANNER -->
<div class="insights-banner" v-if="insights && insights.savingPercent !== undefined && !insightsLoading">
  <div class="insights-content">
    <div class="insight-icon-wrapper">
      <div class="insight-icon-pulse"></div>
      <div class="insight-icon">
        <i class="bi bi-lightbulb-fill"></i>
      </div>
    </div>
    
    <div class="insight-main">
      <div class="insight-header">
        <h3>💡 Smart Insights</h3>
        <span class="insight-badge">This Month</span>
      </div>
      <p class="insight-message">{{ insights.summary }}</p>
      <p class="insight-comparison" v-if="insights.comparison">
        <i class="bi bi-graph-up-arrow"></i>
        {{ insights.comparison }}
      </p>
    </div>

    <div class="insight-stats-wrapper">
      <div class="savings-circle" :class="savingsClass">
        <svg viewBox="0 0 100 100">
          <circle cx="50" cy="50" r="40" class="circle-bg"/>
          <circle 
            cx="50" 
            cy="50" 
            r="40" 
            class="circle-progress"
            :style="{ strokeDasharray: `${Math.min(insights.savingPercent * 2.51, 251.2)} 251.2` }"
          />
        </svg>
        <div class="circle-content">
          <span class="percentage">{{ insights.savingPercent.toFixed(1) }}%</span>
          <span class="label">Saved</span>
        </div>
      </div>

      <div class="mini-stats">
        <div class="mini-stat">
          <i class="bi bi-arrow-down-circle income-icon-mini"></i>
          <div>
            <span class="mini-label">Income</span>
            <span class="mini-value income">NPR {{ formatNumber(insights.thisMonthIncome) }}</span>
          </div>
        </div>
        <div class="mini-stat">
          <i class="bi bi-arrow-up-circle expense-icon-mini"></i>
          <div>
            <span class="mini-label">Expenses</span>
            <span class="mini-value expense">NPR {{ formatNumber(insights.thisMonthExpense) }}</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>


<div class="insights-banner insights-loading" v-else-if="insightsLoading">
  <div class="loading-spinner"></div>
  <span>Loading insights...</span>
</div>
   
    <div class="row g-4 mb-4">
      <div class="col-lg-4 col-md-6" v-for="(card, idx) in summaryCards" :key="card.title">
        <div class="stat-card" :class="`stat-card-${idx + 1}`">
          <div class="stat-icon">
            <i :class="card.icon"></i>
          </div>
          <div class="stat-content">
            <p class="stat-label">{{ card.title }}</p>
            <h3 class="stat-value">NPR {{ formatNumber(card.value) }}</h3>
            <div class="stat-change" :class="card.change >= 0 ? 'positive' : 'negative'">
              <i :class="card.change >= 0 ? 'bi bi-arrow-up' : 'bi bi-arrow-down'"></i>
              <span>{{ Math.abs(card.change) }}% from last month</span>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="row g-4">
      <div class="col-lg-8">
        <div class="chart-card">
          <div class="card-header-custom">
            <h5 class="card-title-custom">
              <i class="bi bi-graph-up"></i>
              Income vs Expenses
            </h5>
            <div class="chart-controls">
              <button class="btn-chart-filter active">Week</button>
              <button class="btn-chart-filter">Month</button>
              <button class="btn-chart-filter">Year</button>
            </div>
          </div>
          <div class="chart-wrapper">
            <canvas id="financeChart"></canvas>
          </div>
        </div>
      </div>

      <div class="col-lg-4">
        <div class="category-card">
          <div class="card-header-custom">
            <h5 class="card-title-custom">
              <i class="bi bi-pie-chart"></i>
              Top Expense Categories
            </h5>
          </div>
          
          <div class="pie-wrapper">
            <canvas id="categoryPie"></canvas>
          </div>

          <div class="category-list">
            <div
              class="category-item"
              v-for="cat in topCategories"
              :key="cat.name"
            >
              <div class="category-info">
                <span
                  class="category-dot"
                  :style="{ backgroundColor: cat.color }"
                ></span>
                <span class="category-name">{{ cat.name }}</span>
              </div>
              <div class="category-amount">
                <span class="amount">NPR {{ formatNumber(cat.amount) }}</span>
                <span class="percentage">{{ cat.percentage }}%</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="transactions-card mt-4">
      <div class="card-header-custom">
        <h5 class="card-title-custom">
          <i class="bi bi-clock-history"></i>
          Recent Transactions
        </h5>
        <a href="#" class="view-all-link">View All <i class="bi bi-arrow-right"></i></a>
      </div>
      <div class="table-responsive">
        <table class="transactions-table">
          <thead>
            <tr>
              <th>Date</th>
              <th>Description</th>
              <th>Category</th>
              <th>Amount</th>
              <th>Type</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="transaction in recentTransactions" :key="transaction.id">
              <td>
                <span class="date-cell">{{ formatDate(transaction.date) }}</span>
              </td>
              <td>
                <div class="description-cell">
                  <div class="description-icon" :class="transaction.type === 'income' ? 'income-icon' : 'expense-icon'">
                    <i :class="getTransactionIcon(transaction)"></i>
                  </div>
                  <span>{{ transaction.description }}</span>
                </div>
              </td>
              <td>
                <span class="category-badge">{{ transaction.category }}</span>
              </td>
              <td>
                <span class="amount-cell" :class="transaction.type === 'income' ? 'positive' : 'negative'">
                  {{ transaction.type === 'income' ? '+' : '-' }}NPR {{ formatNumber(transaction.amount) }}
                </span>
              </td>
              <td>
                <span class="status-badge" :class="transaction.type === 'income' ? 'income-badge' : 'expense-badge'">
                  {{ transaction.type === 'income' ? 'Income' : 'Expense' }}
                </span>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script>
import { onMounted, ref, computed,watch } from "vue";
import reportService from "../services/reportService";
import expenseService from "../services/expenseService";
import incomeService from "../services/incomeService";
import insightsService from "../services/insightsService"; // 🌟 NEW IMPORT
import Chart from "chart.js/auto";

export default {
  name: "Dashboard",
  setup() {
    const summaryCards = ref([
      { 
        title: "Total Income", 
        value: 0, 
        icon: "bi bi-wallet2",
        change: 12.5
      },
      { 
        title: "Total Expenses", 
        value: 0, 
        icon: "bi bi-credit-card",
        change: -8.2
      },
      { 
        title: "Balance", 
        value: 0, 
        icon: "bi bi-bank",
        change: 15.3
      },
    ]);

    const recentExpenses = ref([]);
    const recentIncomes = ref([]);
    const recentTransactions = ref([]);
    const topCategories = ref([]);
    const isDownloading = ref(false);
    
    // 🌟 NEW: Insights state
    const insights = ref(null);
    const insightsLoading = ref(true);

      watch(insights, (newVal) => {
      console.log('📊 Insights data changed:', newVal);
      console.log('📊 SavingPercent:', newVal?.SavingPercent);
      console.log('📊 Condition check:', newVal && newVal.SavingPercent !== undefined);
    }, { immediate: true, deep: true });
     watch(insightsLoading, (newVal) => {
      console.log('⏳ Insights loading:', newVal);
    }, { immediate: true });

    const currentDate = computed(() => {
      const options = { year: 'numeric', month: 'long', day: 'numeric' };
      return new Date().toLocaleDateString('en-US', options);
    });

    // 🌟 NEW: Computed property for savings class
    const savingsClass = computed(() => {
      if (!insights.value) return '';
      if (insights.value.SavingPercent > 20) return 'excellent';
      if (insights.value.SavingPercent > 0) return 'good';
      return 'warning';
    });

    const formatNumber = (num) => {
      return parseFloat(num || 0).toLocaleString('en-IN', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    };

    const formatDate = (dateString) => {
      if (!dateString) return '';
      const date = new Date(dateString);
      const options = { month: 'short', day: 'numeric' };
      return date.toLocaleDateString('en-US', options);
    };

    const getTransactionIcon = (transaction) => {
      if (transaction.type === 'income') {
        return 'bi bi-arrow-down-circle';
      }
      
      const icons = {
        'Food': 'bi bi-cup-hot',
        'Transport': 'bi bi-car-front',
        'Shopping': 'bi bi-bag',
        'Entertainment': 'bi bi-film',
        'Bills': 'bi bi-receipt',
        'Health': 'bi bi-heart-pulse',
        'Education': 'bi bi-book',
      };
      return icons[transaction.category] || 'bi bi-tag';
    };

    const calculateTopCategories = (expenses) => {
      const categoryTotals = {};
      expenses.forEach(expense => {
        const category = expense.categoryName || 'Others';
        categoryTotals[category] = (categoryTotals[category] || 0) + parseFloat(expense.amount || 0);
      });

      const total = Object.values(categoryTotals).reduce((sum, val) => sum + val, 0);
      const colors = ['#667eea', '#f093fb', '#4facfe', '#43e97b', '#fa709a', '#feca57', '#48dbfb', '#ff6b6b'];

      const categories = Object.entries(categoryTotals)
        .map(([name, amount], index) => ({
          name,
          amount,
          percentage: total > 0 ? Math.round((amount / total) * 100) : 0,
          color: colors[index % colors.length]
        }))
        .sort((a, b) => b.amount - a.amount)
        .slice(0, 5);

      return categories;
    };

    // 🌟 NEW: Fetch insights
    const fetchInsights = async () => {
      try {
        insightsLoading.value = true;
        console.log('🔄 Starting to fetch insights...');
        
        const response = await insightsService.getMonthlyInsights();
         console.log('✅ API Response:', response);
        console.log('✅ Response data:', response.data);
           console.log('🔍 SavingPercent value:', response.data.SavingPercent);
    console.log('🔍 SavingPercent type:', typeof response.data.SavingPercent);
    console.log('🔍 Is undefined?:', response.data.SavingPercent === undefined);
    console.log('🔍 All keys:', Object.keys(response.data));
        insights.value = response.data;
        console.log('✅ Insights value after assignment:', insights.value);
      } catch (err) {
        console.error('Error fetching insights:', err);
        console.error('❌ Error response:', err.response);
      } finally {
        insightsLoading.value = false;
         console.log('✅ Insights loading complete. Final state:', {
          insights: insights.value,
          loading: insightsLoading.value
         });
      }
    };
    

    const downloadExcelReport = async () => {
      isDownloading.value = true;
      try {
        const response = await reportService.downloadExcelReport();
        
        const url = window.URL.createObjectURL(new Blob([response.data]));
        const link = document.createElement('a');
        link.href = url;
        link.download = 'BudgetReport.xlsx';
        
        document.body.appendChild(link);
        link.click();
        
        document.body.removeChild(link);
        window.URL.revokeObjectURL(url);
        
        console.log('Report downloaded successfully');
      } catch (error) {
        console.error('Error downloading report:', error);
        alert('Failed to download report. Please try again.');
      } finally {
        isDownloading.value = false;
      }
    };

    const fetchData = async () => {
      try {
        const expensesRes = await expenseService.getExpenses();
        recentExpenses.value = expensesRes.data.map(e => ({
          ...e,
          date: e.date ? e.date.split('T')[0] : '',
          type: 'expense',
          description: e.description || e.categoryName,
          category: e.categoryName
        }));

        const incomesRes = await incomeService.getIncomes();
        recentIncomes.value = incomesRes.data.map(i => ({
          ...i,
          date: i.date ? i.date.split('T')[0] : '',
          type: 'income',
          description: i.notes || i.source || 'Income',
          category: i.source || 'Income'
        }));

        const totalExpenses = recentExpenses.value.reduce((sum, e) => sum + parseFloat(e.amount || 0), 0);
        const totalIncome = recentIncomes.value.reduce((sum, i) => sum + parseFloat(i.amount || 0), 0);
        const balance = totalIncome - totalExpenses;

        summaryCards.value[0].value = totalIncome;
        summaryCards.value[1].value = totalExpenses;
        summaryCards.value[2].value = balance;

        if (balance > 0) {
          summaryCards.value[2].change = 15.3;
        } else {
          summaryCards.value[2].change = -5.2;
        }

        const allTransactions = [...recentExpenses.value, ...recentIncomes.value];
        allTransactions.sort((a, b) => new Date(b.date) - new Date(a.date));
        recentTransactions.value = allTransactions.slice(0, 8);

        topCategories.value = calculateTopCategories(recentExpenses.value);

      } catch (err) {
        console.error("Error fetching data:", err);
      }
    };

    const renderChart = () => {
      const ctx = document.getElementById("financeChart");
      if (!ctx) return;

      const dateMap = {};
      
      recentExpenses.value.forEach(e => {
        if (!dateMap[e.date]) {
          dateMap[e.date] = { income: 0, expense: 0 };
        }
        dateMap[e.date].expense += parseFloat(e.amount || 0);
      });

      recentIncomes.value.forEach(i => {
        if (!dateMap[i.date]) {
          dateMap[i.date] = { income: 0, expense: 0 };
        }
        dateMap[i.date].income += parseFloat(i.amount || 0);
      });

      const sortedDates = Object.keys(dateMap).sort();
      const labels = sortedDates.map(date => formatDate(date));
      const incomeData = sortedDates.map(date => dateMap[date].income);
      const expenseData = sortedDates.map(date => dateMap[date].expense);

      const gradient1 = ctx.getContext("2d").createLinearGradient(0, 0, 0, 400);
      gradient1.addColorStop(0, "rgba(67, 233, 123, 0.6)");
      gradient1.addColorStop(1, "rgba(67, 233, 123, 0.1)");

      const gradient2 = ctx.getContext("2d").createLinearGradient(0, 0, 0, 400);
      gradient2.addColorStop(0, "rgba(250, 112, 154, 0.6)");
      gradient2.addColorStop(1, "rgba(250, 112, 154, 0.1)");

      new Chart(ctx, {
        type: "line",
        data: {
          labels: labels.length > 0 ? labels : ['No data'],
          datasets: [
            {
              label: "Income",
              data: incomeData.length > 0 ? incomeData : [0],
              backgroundColor: gradient1,
              borderColor: "#43e97b",
              borderWidth: 3,
              fill: true,
              tension: 0.4,
              pointBackgroundColor: "#fff",
              pointBorderColor: "#43e97b",
              pointBorderWidth: 2,
              pointRadius: 5,
              pointHoverRadius: 7,
            },
            {
              label: "Expenses",
              data: expenseData.length > 0 ? expenseData : [0],
              backgroundColor: gradient2,
              borderColor: "#fa709a",
              borderWidth: 3,
              fill: true,
              tension: 0.4,
              pointBackgroundColor: "#fff",
              pointBorderColor: "#fa709a",
              pointBorderWidth: 2,
              pointRadius: 5,
              pointHoverRadius: 7,
            }
          ]
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: {
            legend: {
              display: true,
              position: 'top',
              labels: {
                usePointStyle: true,
                padding: 15,
                font: {
                  size: 12,
                  weight: '500'
                }
              }
            },
            tooltip: {
              backgroundColor: "#1f2937",
              padding: 12,
              borderRadius: 8,
              titleColor: "#fff",
              bodyColor: "#fff",
              displayColors: true,
              callbacks: {
                label: function(context) {
                  return context.dataset.label + ': NPR ' + formatNumber(context.parsed.y);
                }
              }
            }
          },
          scales: {
            y: {
              beginAtZero: true,
              grid: {
                color: "rgba(0, 0, 0, 0.05)",
              },
              ticks: {
                callback: function(value) {
                  return 'NPR ' + value;
                }
              }
            },
            x: {
              grid: {
                display: false
              }
            }
          }
        }
      });

      const pieCtx = document.getElementById('categoryPie');
      if (pieCtx) {
        new Chart(pieCtx, {
          type: 'doughnut',
          data: {
            labels: topCategories.value.map(c => c.name),
            datasets: [{
              data: topCategories.value.map(c => c.amount),
              backgroundColor: topCategories.value.map(c => c.color),
              borderWidth: 0,
              hoverOffset: 8
            }]
          },
          options: {
            responsive: true,
            maintainAspectRatio: false,
            cutout: '65%',
            plugins: { legend: { display: false } }
          }
        });
      }
    };

    onMounted(async () => {
      await Promise.all([fetchData(), fetchInsights()]); // 🌟 Fetch both in parallel
      setTimeout(() => {
        renderChart();
      }, 100);
    });

    return { 
      summaryCards, 
      recentTransactions,
      topCategories,
      currentDate,
      formatNumber,
      formatDate,
      getTransactionIcon,
      downloadExcelReport,
      isDownloading,
      // 🌟 NEW returns
      insights,
      insightsLoading,
      savingsClass
    };
  }
};
</script>

<style scoped>
@import url('https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.0/font/bootstrap-icons.css');

.dashboard-container {
  padding: 2rem;
  background: linear-gradient(135deg, #f5f7fa 0%, #f0f3f7 100%);
  min-height: 100vh;
}

.dashboard-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
}

.dashboard-title {
  font-size: 2rem;
  font-weight: 700;
  color: #1f2937;
  margin: 0;
}

.dashboard-subtitle {
  color: #6b7280;
  margin: 0.25rem 0 0 0;
  font-size: 0.95rem;
}

.header-actions {
  display: flex;
  gap: 1rem;
  align-items: center;
}

.download-btn {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.75rem 1.5rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  border-radius: 12px;
  font-size: 0.875rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.3);
}

.download-btn:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(102, 126, 234, 0.4);
}

.download-btn:active:not(:disabled) {
  transform: translateY(0);
}

.download-btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.download-btn i {
  font-size: 1rem;
}

.date-badge {
  background: white;
  padding: 0.5rem 1rem;
  border-radius: 50px;
  font-size: 0.875rem;
  color: #4b5563;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
}

.date-badge i {
  margin-right: 0.5rem;
}

/* 🌟 INSIGHTS BANNER STYLES */
.insights-banner {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-radius: 20px;
  padding: 2rem;
  margin-bottom: 2rem;
  box-shadow: 0 8px 32px rgba(102, 126, 234, 0.3);
  position: relative;
  overflow: hidden;
  animation: slideIn 0.6s ease-out;
}

@keyframes slideIn {
  from {
    opacity: 0;
    transform: translateY(-20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.insights-banner::before {
  content: '';
  position: absolute;
  top: -50%;
  right: -10%;
  width: 300px;
  height: 300px;
  background: rgba(255, 255, 255, 0.1);
  border-radius: 50%;
  filter: blur(60px);
}

.insights-content {
  display: grid;
  grid-template-columns: auto 1fr auto;
  gap: 2rem;
  align-items: center;
  position: relative;
  z-index: 1;
}

.insight-icon-wrapper {
  position: relative;
}

.insight-icon-pulse {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 80px;
  height: 80px;
  background: rgba(255, 255, 255, 0.3);
  border-radius: 50%;
  animation: pulse 2s ease-in-out infinite;
}

@keyframes pulse {
  0%, 100% {
    transform: translate(-50%, -50%) scale(1);
    opacity: 0.6;
  }
  50% {
    transform: translate(-50%, -50%) scale(1.2);
    opacity: 0.3;
  }
}

.insight-icon {
  width: 70px;
  height: 70px;
  background: rgba(255, 255, 255, 0.25);
  backdrop-filter: blur(10px);
  border-radius: 18px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 2rem;
  color: white;
  position: relative;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
}

.insight-main {
  flex: 1;
}

.insight-header {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-bottom: 0.75rem;
}

.insight-header h3 {
  margin: 0;
  color: white;
  font-size: 1.25rem;
  font-weight: 600;
}

.insight-badge {
  background: rgba(255, 255, 255, 0.25);
  backdrop-filter: blur(10px);
  padding: 0.25rem 0.75rem;
  border-radius: 20px;
  font-size: 0.75rem;
  color: white;
  font-weight: 500;
}

.insight-message {
  color: white;
  font-size: 1.125rem;
  font-weight: 500;
  margin: 0 0 0.5rem 0;
  line-height: 1.5;
}

.insight-comparison {
  color: rgba(255, 255, 255, 0.9);
  font-size: 0.875rem;
  margin: 0;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.insight-comparison i {
  font-size: 1rem;
}

.insight-stats-wrapper {
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
}

.savings-circle {
  position: relative;
  width: 130px;
  height: 130px;
}

.savings-circle svg {
  width: 100%;
  height: 100%;
  transform: rotate(-90deg);
}

.circle-bg {
  fill: none;
  stroke: rgba(255, 255, 255, 0.2);
  stroke-width: 8;
}

.circle-progress {
  fill: none;
  stroke-width: 8;
  transition: stroke-dasharray 1s ease;
}

.savings-circle.excellent .circle-progress {
  stroke: #2ecc71;
}

.savings-circle.good .circle-progress {
  stroke: #f39c12;
}

.savings-circle.warning .circle-progress {
  stroke: #e74c3c;
}

.circle-content {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  text-align: center;
}

.circle-content .percentage {
  display: block;
  color: white;
  font-size: 1.75rem;
  font-weight: 700;
  line-height: 1;
}

.circle-content .label {
  display: block;
  color: rgba(255, 255, 255, 0.8);
  font-size: 0.75rem;
  margin-top: 0.25rem;
  text-transform: uppercase;
  letter-spacing: 1px;
}

.mini-stats {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.mini-stat {
  background: rgba(255, 255, 255, 0.15);
  backdrop-filter: blur(10px);
  padding: 0.75rem 1rem;
  border-radius: 12px;
  display: flex;
  align-items: center;
  gap: 0.75rem;
  min-width: 180px;
}

.mini-stat i {
  font-size: 1.5rem;
  color: white;
}

.income-icon-mini {
  color: #2ecc71;
}

.expense-icon-mini {
  color: #e74c3c;
}

.mini-label {
  display: block;
  color: rgba(255, 255, 255, 0.8);
  font-size: 0.7rem;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  margin-bottom: 0.125rem;
}

.mini-value {
  display: block;
  color: white;
  font-size: 0.875rem;
  font-weight: 600;
}

.mini-value.income {
  color: #2ecc71;
}

.mini-value.expense {
  color: #ff6b6b;
}

/* Loading state */
.insights-loading {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 1rem;
  font-size: 0.95rem;
  color: white;
  min-height: 120px;
}

.loading-spinner {
  width: 24px;
  height: 24px;
  border: 3px solid rgba(255, 255, 255, 0.3);
  border-top-color: white;
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}

/* 🌟 END OF INSIGHTS STYLES */


.stat-card {
  background: white;
  border-radius: 16px;
  padding: 1.5rem;
  display: flex;
  gap: 1rem;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
  transition: transform 0.3s ease, box-shadow 0.3s ease;
  position: relative;
  overflow: hidden;
}

.stat-card::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 4px;
  background: linear-gradient(90deg, #667eea 0%, #764ba2 100%);
}

.stat-card-1::before {
  background: linear-gradient(90deg, #43e97b 0%, #38f9d7 100%);
}

.stat-card-2::before {
  background: linear-gradient(90deg, #fa709a 0%, #fee140 100%);
}

.stat-card-3::before {
  background: linear-gradient(90deg, #4facfe 0%, #00f2fe 100%);
}

.stat-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.1);
}

.stat-icon {
  width: 60px;
  height: 60px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.stat-card-1 .stat-icon {
  background: linear-gradient(135deg, #43e97b 0%, #38f9d7 100%);
}

.stat-card-2 .stat-icon {
  background: linear-gradient(135deg, #fa709a 0%, #fee140 100%);
}

.stat-card-3 .stat-icon {
  background: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%);
}

.stat-content {
  flex: 1;
}

.stat-label {
  font-size: 0.875rem;
  color: #6b7280;
  margin: 0 0 0.5rem 0;
  font-weight: 500;
}

.stat-value {
  font-size: 1.75rem;
  font-weight: 700;
  color: #1f2937;
  margin: 0 0 0.5rem 0;
}

.stat-change {
  font-size: 0.75rem;
  display: flex;
  align-items: center;
  gap: 0.25rem;
}

.stat-change.positive {
  color: #10b981;
}

.stat-change.negative {
  color: #ef4444;
}

/* Chart Card */
.chart-card, .category-card, .transactions-card {
  background: white;
  border-radius: 16px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
  overflow: hidden;
}

.card-header-custom {
  padding: 1.5rem;
  border-bottom: 1px solid #f3f4f6;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.card-title-custom {
  font-size: 1.125rem;
  font-weight: 600;
  color: #1f2937;
  margin: 0;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.chart-controls {
  display: flex;
  gap: 0.5rem;
}

.btn-chart-filter {
  padding: 0.375rem 0.875rem;
  border: 1px solid #e5e7eb;
  background: white;
  border-radius: 8px;
  font-size: 0.875rem;
  cursor: pointer;
  transition: all 0.3s ease;
}

.btn-chart-filter:hover, .btn-chart-filter.active {
  background: #667eea;
  color: white;
  border-color: #667eea;
}

.chart-wrapper {
  padding: 1.5rem;
  height: 300px;
}

/* Category List */
.category-list {
  padding: 1rem;
}

.category-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem;
  border-radius: 12px;
  margin-bottom: 0.5rem;
  transition: background 0.2s ease;
}

.category-item:hover {
  background: #f9fafb;
}

.category-info {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.category-dot {
  width: 12px;
  height: 12px;
  border-radius: 50%;
}

.category-name {
  font-size: 0.875rem;
  color: #4b5563;
  font-weight: 500;
}

.category-amount {
  text-align: right;
}

.category-amount .amount {
  display: block;
  font-weight: 600;
  color: #1f2937;
  font-size: 0.875rem;
}

.category-amount .percentage {
  font-size: 0.75rem;
  color: #9ca3af;
}

/* Transactions Table */
.table-responsive {
  overflow-x: auto;
}

.transactions-table {
  width: 100%;
  border-collapse: collapse;
}

.transactions-table thead th {
  padding: 1rem 1.5rem;
  text-align: left;
  font-size: 0.75rem;
  font-weight: 600;
  color: #6b7280;
  text-transform: uppercase;
  letter-spacing: 0.05em;
  border-bottom: 1px solid #f3f4f6;
}

.transactions-table tbody tr {
  border-bottom: 1px solid #f3f4f6;
  transition: background 0.2s ease;
}

.transactions-table tbody tr:hover {
  background: #f9fafb;
}

.transactions-table tbody td {
  padding: 1rem 1.5rem;
}

.date-cell {
  font-size: 0.875rem;
  color: #6b7280;
}

.description-cell {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.description-icon {
  width: 36px;
  height: 36px;
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 0.875rem;
}

.income-icon {
  background: linear-gradient(135deg, #43e97b 0%, #38f9d7 100%);
}

.expense-icon {
  background: linear-gradient(135deg, #fa709a 0%, #fee140 100%);
}

.category-badge {
  padding: 0.375rem 0.75rem;
  background: #f3f4f6;
  border-radius: 20px;
  font-size: 0.75rem;
  font-weight: 500;
  color: #4b5563;
}

.amount-cell {
  font-weight: 600;
  font-size: 0.875rem;
}

.amount-cell.positive {
  color: #10b981;
}

.amount-cell.negative {
  color: #ef4444;
}

.status-badge {
  padding: 0.375rem 0.75rem;
  border-radius: 20px;
  font-size: 0.75rem;
  font-weight: 500;
}

.income-badge {
  background: #d1fae5;
  color: #065f46;
}

.expense-badge {
  background: #fee2e2;
  color: #991b1b;
}

.view-all-link {
  color: #667eea;
  text-decoration: none;
  font-size: 0.875rem;
  font-weight: 500;
  display: flex;
  align-items: center;
  gap: 0.25rem;
  transition: gap 0.3s ease;
}

.view-all-link:hover {
  gap: 0.5rem;
}

.pie-wrapper {
  height: 220px;
  padding: 1.5rem 1.5rem 0;
  display: flex;
  align-items: center;
  justify-content: center;
}

/* Responsive */
@media (max-width: 768px) {
  .dashboard-container {
    padding: 1rem;
  }

  .dashboard-header {
    flex-direction: column;
    align-items: flex-start;
    gap: 1rem;
  }

  .header-actions {
    width: 100%;
    flex-direction: column-reverse;
  }

  .download-btn {
    width: 100%;
    justify-content: center;
  }

  .date-badge {
    width: 100%;
    text-align: center;
  }

  /* 🌟 Mobile responsive for insights */
  .insights-banner {
    padding: 1.5rem;
  }

  .insights-content {
    grid-template-columns: 1fr;
    gap: 1.5rem;
  }

  .insight-icon-wrapper {
    display: none;
  }

  .insight-stats-wrapper {
    flex-direction: row;
    justify-content: space-between;
  }

  .savings-circle {
    width: 110px;
    height: 110px;
  }

  .circle-content .percentage {
    font-size: 1.5rem;
  }

  .mini-stats {
    flex: 1;
  }

  .mini-stat {
    min-width: auto;
  }

  .stat-card {
    flex-direction: column;
  }

  .chart-wrapper {
    height: 250px;
  }
}
</style>