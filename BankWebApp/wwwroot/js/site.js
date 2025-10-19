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

// DOMContentLoaded event
document.addEventListener("DOMContentLoaded", loadView);
