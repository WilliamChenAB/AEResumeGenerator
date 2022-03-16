/// <reference path="oidc-client.js" />

function log() {
    document.getElementById('results').innerText = '';

    Array.prototype.forEach.call(arguments, function (msg) {
        if (msg instanceof Error) {
            msg = "Error: " + msg.message;
        }
        else if (typeof msg !== 'string') {
            msg = JSON.stringify(msg, null, 2);
        }
        document.getElementById('results').innerText += msg + '\r\n';
    });
}

document.getElementById("login").addEventListener("click", login, false);
document.getElementById("api").addEventListener("click", api, false);
document.getElementById("logout").addEventListener("click", logout, false);

var config = {
    authority: "https://localhost:5003",
    client_id: "js",
    redirect_uri: "https://localhost:5002/callback.html",
    response_type: "id_token token",
    scope: "openid ae-resume-api",
    //end_session_endpoint: "https://localhost:5003/connect/endsession",
    //automaticSilentRenew = true,
    //revokeAccessTokenOnSignout = true,
    post_logout_redirect_uri: "https://localhost:5002/index.html",
    automaticSilentRenew: true,
    filterProtocolClaims: true,
};
var mgr = new Oidc.UserManager(config);

mgr.events.addUserSignedOut(function () {
    log("User signed out of IdentityServer");
});

mgr.getUser().then(function (user) {
    if (user) {
        log("User logged in", user.profile);
    }
    else {
        log("User not logged in");
    }
});

function login() {
    mgr.signinRedirect();
}

function api() {
    mgr.getUser().then(function (user) {
        var url = "https://localhost:5001/identity";

        var xhr = new XMLHttpRequest();
        xhr.open("GET", url);
        xhr.onload = function () {
            log(xhr.status, JSON.parse(xhr.responseText));
        }
        xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
        xhr.send();
    });
}

function logout() {
    mgr.getUser().then(function (user) {
        mgr.signoutRedirect({ id_token_hint: user.id_token });
    });
}