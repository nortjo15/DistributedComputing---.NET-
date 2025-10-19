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

// DOMContentLoaded event
document.addEventListener("DOMContentLoaded", function() {
    loadView();
    displaySelectedRole();
});
