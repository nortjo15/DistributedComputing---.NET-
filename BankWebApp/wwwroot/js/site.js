// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let selectedRole = null;

function loadView(status, role = null) {
    var apiUrl = '/api/login/roleselectionview';

    if (role != null) {
        selectedRole = role;
    }

    if (status === "default")
        apiUrl = '/api/login/defaultview';

    if (status === "login") {
        apiUrl = '/api/login/defaultview';
    }

    if (status === "authview") {
        if (selectedRole == null) {
            apiUrl = '/api/login/error';
        } else {
            apiUrl = '/api/login/authview/' + selectedRole;
        }
    }

    if (status === "error")
        apiUrl = '/api/login/error';

    if (status === "about")
        apiUrl = '/api/about/view';

    if (status === "logout")
        apiUrl = '/api/logout';

    // Check if main element exists
    const mainElement = document.getElementById('main');
    if (!mainElement) {
        console.error('Element with id "main" not found');
        return;
    }

    fetch(apiUrl)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(data => {
            mainElement.innerHTML = data;
            
            // If loading admin dashboard, initialize DataTables
            if (status === "authview" && selectedRole === "admin") {
                setTimeout(() => {
                    initializeAdminDashboard();
                }, 100);
            }
            
            if (status === "logout") {
                // After logout, navigate back to role selection
                selectedRole = null; // Clear selected role
                setTimeout(() => loadView(), 100); // Load role selection view
            }
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
}

function performAuth() {
    var name = document.getElementById('SName').value;
    var password = document.getElementById('SPass').value;
    var role = selectedRole;
    
    if (!role) {
        alert('Please select a role first by going back to the role selection.');
        return;
    }
    
    if (!name || !password) {
        alert('Please enter both username and password.');
        return;
    }
    
    var data = {
        Username: name,
        Password: password,
        Role: role
    };
    
    const apiUrl = '/api/login/auth';

    const headers = {
        'Content-Type': 'application/json',
    };

    const requestOptions = {
        method: 'POST',
        headers: headers,
        body: JSON.stringify(data)
    };

    fetch(apiUrl, requestOptions)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            const jsonObject = data;
            
            if (jsonObject.login) {
                loadView("authview", jsonObject.role);
            } else {
                alert('Login failed. Please check your credentials.');
                loadView("error");
            }
        })
        .catch(error => {
            console.error('Authentication fetch error:', error);
            alert('Authentication error: ' + error.message);
            loadView("error");
        });
}

function performLogout() {
    if (confirm('Are you sure you want to logout?')) {
        loadView("logout");
    }
}

function createAccount() {
    const username = document.getElementById('username').value;
    const accountName = document.getElementById('accountName').value;
    const email = document.getElementById('email').value;
    const balance = parseFloat(document.getElementById('balance').value);

    // Validate required fields
    if (!username || !accountName) {
        alert('Username and Account Name are required!');
        return;
    }

    const accountData = {
        Username: username,
        Name: accountName,
        Email: email,
        Balance: balance
    };

    fetch('/Admin/CreateAccount', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(accountData)
    })
    .then(response => {
        return response.text();
    })
    .then(html => {
        document.getElementById('main').innerHTML = html;
        
        // Reinitialize DataTables after content update
        setTimeout(() => {
            initializeAdminDashboard();
        }, 100);
        
        // Clear the form inputs only if successful
        if (html.includes('Account created successfully') || html.includes('alert-success')) {
            document.getElementById('username').value = '';
            document.getElementById('accountName').value = '';
            document.getElementById('email').value = '';
            document.getElementById('balance').value = '0';
        }
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Error creating account: ' + error.message);
    });
}

function deleteAccount(accountNumber) {
    if (!confirm(`Are you sure you want to delete account ${accountNumber}?`)) {
        return;
    }

    fetch('/Admin/DeleteAccount', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ AccountNumber: accountNumber })
    })
    .then(response => {
        return response.text();
    })
    .then(html => {
        document.getElementById('main').innerHTML = html;
        
        // Reinitialize DataTables after content update
        setTimeout(() => {
            initializeAdminDashboard();
        }, 100);
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Error deleting account: ' + error.message);
    });
}

function updateAccount(accountNumber, username, email) {
    const balanceInput = document.getElementById(`balance-${accountNumber}`);
    const newBalance = parseFloat(balanceInput.value);

    if (isNaN(newBalance) || newBalance < 0) {
        alert('Please enter a valid balance amount');
        return;
    }

    const accountData = {
        AccountNumber: accountNumber,
        Username: username,
        Email: email,
        Balance: newBalance,
        Name: '' // Name is required but not used in update
    };

    fetch('/Admin/UpdateAccount', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(accountData)
    })
    .then(response => {
        return response.text();
    })
    .then(html => {
        document.getElementById('main').innerHTML = html;
        
        // Reinitialize DataTables after content update
        setTimeout(() => {
            initializeAdminDashboard();
        }, 100);
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Error updating account: ' + error.message);
    });
}

function initializeTransferForm(allAccounts) {
    const toUserSelect = document.getElementById('toUser');
    const toAccountSelect = document.getElementById('toAccount');
    
    if (toUserSelect && toAccountSelect) {
        toUserSelect.addEventListener('change', function () {
            const selectedUser = this.value;
            toAccountSelect.innerHTML = '<option value="">Select Account</option>';
            toAccountSelect.disabled = true;

            if (selectedUser) {
                const accounts = allAccounts.filter(a => a.username === selectedUser || a.Username === selectedUser);
                accounts.forEach(a => {
                    const opt = document.createElement('option');
                    opt.value = a.accountNumber || a.AccountNumber;
                    opt.textContent = `${a.name || a.Name}`;
                    toAccountSelect.appendChild(opt);
                });
                toAccountSelect.disabled = accounts.length === 0;
            }
        });
    }
}

function displaySelectedRole() {
    // Display the selected role in the login form
    if (typeof selectedRole !== 'undefined' && selectedRole) {
        const roleDisplay = document.getElementById('roleDisplay');
        if (roleDisplay) {
            roleDisplay.textContent = `Logging in as: ${selectedRole.toUpperCase()}`;
        }
    }
}

function searchUsers() {
    const username = document.getElementById('searchUsername').value.trim();
    const email = document.getElementById('searchEmail').value.trim();
    const phone = document.getElementById('searchPhone').value.trim();

    if (!username && !email && !phone) {
        alert('Please enter username, email, or phone number to search');
        return;
    }

    // Check if multiple fields are filled
    const filledFields = [username, email, phone].filter(field => field).length;
    if (filledFields > 1) {
        alert('Please search by only one criteria at a time');
        return;
    }

    const searchData = {
        Username: username,
        Email: email,
        Phone: phone
    };

    fetch('/Admin/SearchUsers', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(searchData)
    })
    .then(response => {
        return response.text();
    })
    .then(html => {
        document.getElementById('main').innerHTML = html;
        
        // Reinitialize DataTables after content update
        setTimeout(() => {
            initializeAdminDashboard();
        }, 100);
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Error searching users: ' + error.message);
    });
}

function clearUserSearch() {
    document.getElementById('searchUsername').value = '';
    document.getElementById('searchEmail').value = '';
    document.getElementById('searchPhone').value = '';
    
    // Reload the full admin dashboard
    fetch('/Admin/GetView', {
        method: 'GET'
    })
    .then(response => response.text())
    .then(html => {
        document.getElementById('main').innerHTML = html;
        
        // Reinitialize DataTables after content update
        setTimeout(() => {
            initializeAdminDashboard();
        }, 100);
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Error clearing search: ' + error.message);
    });
}

function searchAccounts() {
    const accountNumber = document.getElementById('searchAccountNumber').value.trim();
    const username = document.getElementById('searchAccountUsername').value.trim();
    const email = document.getElementById('searchAccountEmail').value.trim();

    if (!accountNumber && !username && !email) {
        alert('Please enter account number, username, or email to search');
        return;
    }

    // Check if multiple fields are filled
    const filledFields = [accountNumber, username, email].filter(field => field).length;
    if (filledFields > 1) {
        alert('Please search by only one criteria at a time');
        return;
    }

    const searchData = {
        AccountNumber: accountNumber ? parseInt(accountNumber) : null,
        Username: username,
        Email: email
    };

    // Validate account number if provided
    if (accountNumber && (isNaN(parseInt(accountNumber)) || parseInt(accountNumber) <= 0)) {
        alert('Please enter a valid account number');
        return;
    }

    fetch('/Admin/SearchAccounts', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(searchData)
    })
    .then(response => {
        return response.text();
    })
    .then(html => {
        document.getElementById('main').innerHTML = html;
        
        // Reinitialize DataTables after content update
        setTimeout(() => {
            initializeAdminDashboard();
        }, 100);
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Error searching accounts: ' + error.message);
    });
}

function clearAccountSearch() {
    document.getElementById('searchAccountNumber').value = '';
    document.getElementById('searchAccountUsername').value = '';
    document.getElementById('searchAccountEmail').value = '';
    
    // Reload the full admin dashboard
    fetch('/Admin/GetView', {
        method: 'GET'
    })
    .then(response => response.text())
    .then(html => {
        document.getElementById('main').innerHTML = html;
        
        // Reinitialize DataTables after content update
        setTimeout(() => {
            initializeAdminDashboard();
        }, 100);
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Error clearing search: ' + error.message);
    });
}

// Server-side transaction search functions (alternative to local DataTable search)
function searchTransactions() {
    const transactionId = document.getElementById('searchTransactionId').value.trim();
    const accountNumber = document.getElementById('searchTransactionAccount').value.trim();
    const transactionType = document.getElementById('searchTransactionType').value.trim();

    if (!transactionId && !accountNumber && !transactionType) {
        alert('Please enter transaction ID, account number, or select transaction type to search');
        return;
    }

    // Check if multiple fields are filled
    const filledFields = [transactionId, accountNumber, transactionType].filter(field => field).length;
    if (filledFields > 1) {
        alert('Please search by only one criteria at a time');
        return;
    }

    const searchData = {
        TransactionId: transactionId ? parseInt(transactionId) : null,
        AccountNumber: accountNumber ? parseInt(accountNumber) : null,
        Type: transactionType
    };

    // Validate numeric fields if provided
    if (transactionId && (isNaN(parseInt(transactionId)) || parseInt(transactionId) <= 0)) {
        alert('Please enter a valid transaction ID');
        return;
    }
    if (accountNumber && (isNaN(parseInt(accountNumber)) || parseInt(accountNumber) <= 0)) {
        alert('Please enter a valid account number');
        return;
    }

    fetch('/Admin/SearchTransactions', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(searchData)
    })
    .then(response => {
        return response.text();
    })
    .then(html => {
        document.getElementById('main').innerHTML = html;
        
        // Reinitialize DataTables after content update
        setTimeout(() => {
            initializeAdminDashboard();
        }, 100);
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Error searching transactions: ' + error.message);
    });
}

function clearTransactionSearch() {
    document.getElementById('searchTransactionId').value = '';
    document.getElementById('searchTransactionAccount').value = '';
    document.getElementById('searchTransactionType').value = '';
    
    // Reload the full admin dashboard
    fetch('/Admin/GetView', {
        method: 'GET'
    })
    .then(response => response.text())
    .then(html => {
        document.getElementById('main').innerHTML = html;
        
        // Reinitialize DataTables after content update
        setTimeout(() => {
            initializeAdminDashboard();
        }, 100);
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Error clearing search: ' + error.message);
    });
}

// DOMContentLoaded event
document.addEventListener("DOMContentLoaded", function() {
    loadView();
    displaySelectedRole();
});

// Admin Dashboard DataTable Functions
function initializeAdminDashboard() {
    // Check if jQuery and DataTable are available
    if (typeof $ === 'undefined') {
        console.error('jQuery is not loaded');
        return;
    }
    
    if (typeof $.fn.DataTable === 'undefined') {
        console.error('DataTables is not loaded');
        return;
    }

    // Check if tables exist
    if ($('#usersTable').length === 0) {
        console.error('Users table not found');
        return;
    }

    try {
        // Destroy existing DataTables if they exist
        if (window.adminDataTables) {
            if (window.adminDataTables.users) {
                window.adminDataTables.users.destroy();
            }
            if (window.adminDataTables.accounts) {
                window.adminDataTables.accounts.destroy();
            }
            if (window.adminDataTables.transactions) {
                window.adminDataTables.transactions.destroy();
            }
        }

        // Users DataTable - enable searching but hide search box
        const usersDt = $('#usersTable').DataTable({
            pageLength: 10,
            order: [],
            searching: true,  // Enable searching for filtering
            dom: 'lrtip',     // Hide search box (remove 'f' from dom)
            columnDefs: [
                { targets: -1, orderable: false, searchable: false }
            ]
        });

        // Accounts DataTable - enable searching but hide search box
        const accountsDt = $('#accountsTable').DataTable({
            pageLength: 10,
            order: [[0, 'asc']],
            searching: true,  // Enable searching for filtering
            dom: 'lrtip',     // Hide search box (remove 'f' from dom)
            columnDefs: [
                { targets: 0, type: 'num' },
                { targets: 3, render: $.fn.dataTable.render.number(',', '.', 2) }
            ]
        });

        // Transactions DataTable - enable searching but hide search box
        const txTable = $('#transactionsTable').DataTable({
            pageLength: 25,
            order: [[0, 'desc']],
            searching: true,  // Enable searching for filtering
            dom: 'lrtip',     // Hide search box (remove 'f' from dom)
            columnDefs: [
                { targets: [0, 1], type: 'num' },
                { targets: 3, render: $.fn.dataTable.render.number(',', '.', 2) }
            ]
        });

        // Toggle sections by clicking headers
        $('#usersHeader').off('click').on('click', function(){
            $('#usersSection').slideToggle(150, function(){
                if ($(this).is(':visible')) usersDt.columns.adjust();
            });
        });
        $('#accountsHeader').off('click').on('click', function(){
            $('#accountsSection').slideToggle(150, function(){
                if ($(this).is(':visible')) accountsDt.columns.adjust();
            });
        });
        $('#transactionsHeader').off('click').on('click', function(){
            $('#transactionsSection').slideToggle(150, function(){
                if ($(this).is(':visible')) txTable.columns.adjust();
            });
        });

        // Change Password modal open
        $(document).off('click', '.btn-change-password').on('click', '.btn-change-password', function () {
            const username = $(this).data('username');
            $('#cp-username').val(username);
            $('#cp-username-display').text(username);
            const modal = new bootstrap.Modal(document.getElementById('changePasswordModal'));
            modal.show();
        });

        // Update Account button handler
        $(document).off('click', '.btn-update-account').on('click', '.btn-update-account', function () {
            const accountNumber = $(this).data('account-number');
            const username = $(this).data('username');
            const email = $(this).data('email');
            updateAccount(accountNumber, username, email);
        });

        // Delete Account button handler
        $(document).off('click', '.btn-delete-account').on('click', '.btn-delete-account', function () {
            const accountNumber = $(this).data('account-number');
            deleteAccount(accountNumber);
        });

        // Create Account button handler
        $(document).off('click', '.btn-create-account').on('click', '.btn-create-account', function () {
            createAccount();
        });

        // Change Password submit button handler
        $(document).off('click', '.btn-change-password-submit').on('click', '.btn-change-password-submit', function () {
            changeUserPassword();
        });

        // Store DataTable references globally for search functions
        window.adminDataTables = {
            users: usersDt,
            accounts: accountsDt,
            transactions: txTable
        };
        
    } catch (error) {
        console.error('Error initializing DataTables:', error);
    }
}

// DataTable search functions for local filtering
function searchUsersLocal() {
    const username = $('#searchUsername').val();
    const email = $('#searchEmail').val();
    const phone = $('#searchPhone').val();
    
    if (window.adminDataTables && window.adminDataTables.users) {
        window.adminDataTables.users
            .columns(0).search(username)
            .columns(1).search(email)
            .columns(3).search(phone)
            .draw();
    } else {
        console.error('Users DataTable not available');
    }
}

function clearUserSearchLocal() {
    $('#searchUsername').val('');
    $('#searchEmail').val('');
    $('#searchPhone').val('');
    
    if (window.adminDataTables && window.adminDataTables.users) {
        window.adminDataTables.users
            .columns(0).search('')
            .columns(1).search('')
            .columns(3).search('')
            .draw();
    } else {
        console.error('Users DataTable not available');
    }
}

function searchAccountsLocal() {
    const accountNumber = $('#searchAccountNumber').val();
    const username = $('#searchAccountUsername').val();
    const email = $('#searchAccountEmail').val();
    
    if (window.adminDataTables && window.adminDataTables.accounts) {
        window.adminDataTables.accounts
            .columns(0).search(accountNumber)
            .columns(1).search(username)
            .columns(2).search(email)
            .draw();
    } else {
        console.error('Accounts DataTable not available');
    }
}

function clearAccountSearchLocal() {
    $('#searchAccountNumber').val('');
    $('#searchAccountUsername').val('');
    $('#searchAccountEmail').val('');
    
    if (window.adminDataTables && window.adminDataTables.accounts) {
        window.adminDataTables.accounts
            .columns(0).search('')
            .columns(1).search('')
            .columns(2).search('')
            .draw();
    } else {
        console.error('Accounts DataTable not available');
    }
}

// Transaction search functions for local filtering
function searchTransactionsLocal() {
    const transactionId = $('#searchTransactionId').val();
    const accountNumber = $('#searchTransactionAccount').val();
    const transactionType = $('#searchTransactionType').val();
    
    if (window.adminDataTables && window.adminDataTables.transactions) {
        window.adminDataTables.transactions
            .columns(0).search(transactionId)     // Transaction ID column
            .columns(1).search(accountNumber)     // Account Number column  
            .columns(2).search(transactionType)   // Type column
            .draw();
    } else {
        console.error('Transactions DataTable not available');
    }
}

function clearTransactionSearchLocal() {
    $('#searchTransactionId').val('');
    $('#searchTransactionAccount').val('');
    $('#searchTransactionType').val('');
    
    if (window.adminDataTables && window.adminDataTables.transactions) {
        window.adminDataTables.transactions
            .columns(0).search('')
            .columns(1).search('')
            .columns(2).search('')
            .draw();
    } else {
        console.error('Transactions DataTable not available');
    }
}

function changeUserPassword() {
    const username = document.getElementById('cp-username').value;
    const newPassword = document.getElementById('cp-new-password').value;

    // Validate required fields
    if (!username || !newPassword) {
        alert('Username and new password are required!');
        return;
    }

    if (newPassword.length > 30) {
        alert('Password must be 30 characters or less!');
        return;
    }

    // For now, just close the modal without making any changes
    // This prevents navigation issues while keeping the button functional
    const modal = bootstrap.Modal.getInstance(document.getElementById('changePasswordModal'));
    if (modal) {
        modal.hide();
    }
    
    // Clear the form
    document.getElementById('cp-new-password').value = '';
}

function submitPasswordChange() {
    const form = document.getElementById('changePasswordForm');
    const formData = new FormData(form);

    fetch('/Admin/ChangeUserPassword', {
        method: 'POST',
        body: formData
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            // Show success message (you can use a toast notification or alert)
            alert(data.message);
            
            // Close the modal
            const modal = bootstrap.Modal.getInstance(document.getElementById('changePasswordModal'));
            if (modal) {
                modal.hide();
            }
            
            // Clear the form
            form.reset();
        } else {
            // Show error message
            alert('Error: ' + data.message);
        }
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Error changing password: ' + error.message);
    });
}
