
window.addEventListener("DOMContentLoaded", () => {
    const controlDiv = document.getElementById("controls");
    createButton(controlDiv, "Get Data", getData);
    createButton(controlDiv, "Log In", login);
    createButton(controlDiv, "Log Out", logout);
});


let body;


function setBody() {

    let res = true;

    body = {
        username: document.getElementById("inputLogin").value,
        password: document.getElementById("inputPassword").value
    };

    if (body.username === "" || body.password === "") {
        displayData("No data for login");

        res = false;
    }
    
    return res;

}

async function login() {

    if (setBody() == false)
        return;

    let response = await fetch("/api/account/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(body)
    });
    if (response.ok) {
        displayData("upload data", "ok" );
    } else {
        displayData(`Error: ${response.status}: ${response.statusText}`);
    }

}

async function logout() {    

    if (setBody() == false)
        return;

    let response = await fetch("/api/account/logout", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(body)
    });
    if (response.ok) {        
        displayData("Logged out");
    } else {
        displayData(`Error: ${response.status}: ${response.statusText}`);
    }
        
}

async function getData() {
    let response = await fetch("/api/account/list");

    if (response.ok) {
        let jsonData = await response.json();

        displayData(...jsonData.map(item => `${item.username}, ${item.email}`));

    } else {
        displayData(`Error: ${response.status}: ${response.statusText}`);
    }
}


function displayData(...items) {
    const dataDiv = document.getElementById("data");

    dataDiv.innerHTML = "";

    items.forEach(item => {
        const itemDiv = document.createElement("div");

        itemDiv.innerText = item;
        itemDiv.style.wordWrap = "break-word";
        dataDiv.appendChild(itemDiv);
    })
}

function createButton(parent, label, handler) {
    const button = document.createElement("button");
    button.classList.add("btn", "btn-primary", "m-2");
    button.innerText = label;
    button.onclick = handler;
    parent.appendChild(button);
}
