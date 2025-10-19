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

    console.log('Creating account with:', { username, accountName, email, balance });

    const accountData = {
        Username: username,
        Name: accountName,
        Email: email,
        Balance: balance
    };

    console.log('Sending JSON data:', JSON.stringify(accountData));

    fetch('/Admin/CreateAccount', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(accountData)
    })
    .then(response => {
        console.log('CreateAccount response status:', response.status);
        return response.text();
    })
    .then(html => {
        console.log('Received HTML response, updating main content');
        document.getElementById('main').innerHTML = html;
        
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

    console.log('Deleting account:', accountNumber);

    fetch('/Admin/DeleteAccount', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ AccountNumber: accountNumber })
    })
    .then(response => {
        console.log('DeleteAccount response status:', response.status);
        return response.text();
    })
    .then(html => {
        console.log('Received HTML response, updating main content');
        document.getElementById('main').innerHTML = html;
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

    console.log('Updating account:', { accountNumber, username, email, newBalance });

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
        console.log('UpdateAccount response status:', response.status);
        return response.text();
    })
    .then(html => {
        console.log('Received HTML response, updating main content');
        document.getElementById('main').innerHTML = html;
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

    console.log('Searching users with:', { username, email, phone });

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
        console.log('SearchUsers response status:', response.status);
        return response.text();
    })
    .then(html => {
        console.log('Received HTML response, updating main content');
        document.getElementById('main').innerHTML = html;
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

    console.log('Searching accounts with:', { accountNumber, username, email });

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
        console.log('SearchAccounts response status:', response.status);
        return response.text();
    })
    .then(html => {
        console.log('Received HTML response, updating main content');
        document.getElementById('main').innerHTML = html;
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
    // Users DataTable
    const usersDt = $('#usersTable').DataTable({
        pageLength: 10,
        order: [],
        columnDefs: [
            { targets: -1, orderable: false, searchable: false }
        ]
    });

    // Accounts DataTable
    const accountsDt = $('#accountsTable').DataTable({
        pageLength: 10,
        order: [[0, 'asc']],
        columnDefs: [
            { targets: 0, type: 'num' },
            { targets: 3, render: $.fn.dataTable.render.number(',', '.', 2) }
        ]
    });

    // Transactions DataTable
    const txTable = $('#transactionsTable').DataTable({
        pageLength: 25,
        order: [[0, 'desc']],
        columnDefs: [
            { targets: [0, 1], type: 'num' },
            { targets: 3, render: $.fn.dataTable.render.number(',', '.', 2) }
        ]
    });

    // Toggle sections by clicking headers
    $('#usersHeader').on('click', function(){
        $('#usersSection').slideToggle(150, function(){
            if ($(this).is(':visible')) usersDt.columns.adjust();
        });
    });
    $('#accountsHeader').on('click', function(){
        $('#accountsSection').slideToggle(150, function(){
            if ($(this).is(':visible')) accountsDt.columns.adjust();
        });
    });
    $('#transactionsHeader').on('click', function(){
        $('#transactionsSection').slideToggle(150, function(){
            if ($(this).is(':visible')) txTable.columns.adjust();
        });
    });

    // Change Password modal open
    $(document).on('click', '.btn-change-password', function () {
        const username = $(this).data('username');
        $('#cp-username').val(username);
        $('#cp-username-display').text(username);
        const modal = new bootstrap.Modal(document.getElementById('changePasswordModal'));
        modal.show();
    });

    // Store DataTable references globally for search functions
    window.adminDataTables = {
        users: usersDt,
        accounts: accountsDt,
        transactions: txTable
    };
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
    }
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

    console.log('Searching transactions with:', { transactionId, accountNumber, transactionType });

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
        console.log('SearchTransactions response status:', response.status);
        return response.text();
    })
    .then(html => {
        console.log('Received HTML response, updating main content');
        document.getElementById('main').innerHTML = html;
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
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Error clearing search: ' + error.message);
    });
}
