// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let selectedRole = null;

function loadView(status, role = null) {
    var apiUrl = '/api/login/roleselectionview';

    if (role != null) {
        selectedRole = role;
    }

    if (status == "default")
        apiUrl = '/api/login/defaultview';

    if (status === "login") {
        if (role == null) {
            apiUrl = '/api/login/error';
        } else {
            apiUrl = '/api/login/authview/' + role;
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

    console.log("API URL:", apiUrl)
    fetch(apiUrl)
        .then(response => {
            if (!response.ok) {
                console.log("response", response)
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(data => {
            // Handle the data from the API
            mainElement.innerHTML = data;
            if (status === "logout") {
                const logoutButton = document.getElementById('LogoutButton');
                if (logoutButton) {
                    logoutButton.style.display = "none";
                }
            }
        })
        .catch(error => {
            // Handle any errors that occurred during the fetch
            console.error('Fetch error:', error);
        });
}

function performAuth() {
    var name = document.getElementById('SName').value;
    var password = document.getElementById('SPass').value;
    var role = selectedRole;
    
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
            // Handle the data from the API
            const jsonObject = data;
            if (jsonObject.login) {
                loadView("authview", jsonObject.role);
                const logoutButton = document.getElementById('LogoutButton');
                if (logoutButton) {
                    logoutButton.style.display = "block";
                }
            } else {
                loadView("error");
            }
        })
        .catch(error => {
            // Handle any errors that occurred during the fetch
            console.error('Fetch error:', error);
        });
}

// DOMContentLoaded event
document.addEventListener("DOMContentLoaded", loadView);
