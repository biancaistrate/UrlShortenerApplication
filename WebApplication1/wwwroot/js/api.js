function login(loginInfo) {
    return fetch("http://localhost:5051/login", {
        method: 'post', credentials: 'include',
        body: JSON.stringify(loginInfo),
        headers: {
            'Content-Type': 'application/json'
        },
    })
};

function register(registerInfo) {
    return fetch("http://localhost:5051/register", {
        method: 'put', credentials: 'include',
        body: JSON.stringify(registerInfo),
        headers: {
            'Content-Type': 'application/json'
        },
    })
};

function createUrl(payload) {
    return fetch("http://localhost:5051/tinyurl",
        {
            method: "PUT",
            body: JSON.stringify(payload),
            headers: {
                'Content-Type': 'application/json'
            },
            credentials: 'include'
        })

}

function getAllTinyUrls() {
    return fetch("http://localhost:5051/tinyurl",
        {
            method: "GET",
            headers: {
                'Content-Type': 'application/json'
            },
            credentials: 'include'
        })
}
function getRecentTinyUrls() {
    return fetch("http://localhost:5051/tinyurl/getRecent/3",
        {
            method: "GET",
            headers: {
                'Content-Type': 'application/json'
            },
            credentials: 'include'
        })
}
