
var FetchAPIData = function (ApiUrl) {
    return fetch(ApiUrl)
        .then(response => {
            if (!response.ok) {
                SwalSimpleAlert(`HTTP error! Status: ${response.status}`, "warning");
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            return data;
        })
        .catch(error => {
            SwalSimpleAlert("Fetch error:" + error, "warning");
            throw error;
        });
}

var FetchAPIDataById = function (ApiUrl, id) {
    const apiUrlWithId = `${ApiUrl}?entityName=${encodeURIComponent(id)}`;  // Use query param
    return fetch(apiUrlWithId)
        .then(response => {
            if (!response.ok) {
                SwalSimpleAlert(`HTTP error! Status: ${response.status}`, "warning");
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            return data;
        })
        .catch(error => {
            SwalSimpleAlert("Fetch error:" + error, "warning");
            throw error;
        });
}


var DeleteAPIData = function (ApiUrl) {
    return fetch(ApiUrl, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
        },
    })
        .then(response => {
            if (!response.ok) {
                SwalSimpleAlert(`HTTP error! Status: ${response.status}`, "warning");
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            return data;
        })
        .catch(error => {
            SwalSimpleAlert(error, "warning");
            throw error;
        });
}

var PostAPIData = function (ApiUrl, RequestData) {
    return new Promise((resolve, reject) => {
        const _XMLHttpRequest = new XMLHttpRequest();
        _XMLHttpRequest.open("POST", ApiUrl, true);
        _XMLHttpRequest.setRequestHeader("Content-Type", "application/json");

        _XMLHttpRequest.onload = function () {
            if (_XMLHttpRequest.status >= 200 && _XMLHttpRequest.status < 300) {
                try {
                    const responseData = JSON.parse(_XMLHttpRequest.responseText);
                    resolve(responseData);
                } catch (error) {
                    reject(new Error("Error parsing JSON response"));
                }
            } else {
                reject(new Error(`HTTP error! Status: ${_XMLHttpRequest.status}`));
            }
        };

        _XMLHttpRequest.onerror = function () {
            reject(new Error("Network error"));
        };
        _XMLHttpRequest.send(RequestData);
    });
}

function PostData_AnotherWay(apiUrl, formData) {
    return fetch(apiUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: formData,
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            return data;
        })
        .catch(error => {
            console.error('Fetch error:', error);
            throw error;
        });
}

var SerializeFormToJson = function (formId) {
    const _FormData = new FormData(document.getElementById(formId));
    const jsonData = {};

    _FormData.forEach((value, key) => {
        jsonData[key] = value;
    });
    return JSON.stringify(jsonData);
}