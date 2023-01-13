window.addEventListener('DOMContentLoaded', (event) => {
    setEventListeners();
    setupAuthenticatedAccount();
});

function setEventListeners() {
    var myUrlsSection = document.querySelector('#collapseMyUrls')
    myUrlsSection.addEventListener('show.bs.collapse', fetchAllUrls);

    var currentUserMenu = document.querySelector('#logged-account')
    currentUserMenu.addEventListener('show.bs.collapse', getCurrentUserInfo);
}

function logOutUser() {
    logOut()
        .then(function () {
            document.querySelector("a[href='#signup']").classList.remove("hide");
            document.querySelector("a[href='#signin']").classList.remove("hide");
            document.querySelector("a[href='#logged-account']").classList.add("hide");

            openSignInForm();

    })
        .catch(function () {
            document.querySelector(".error-message").classList.add("show");
        });
}

function getCurrentUserInfo() {
    getCurrentUser()
        .then(function (res) {
            if (!res.ok)
                throw res.statusText;

            return res.json();
        })
        .then(function (data) {
            var toRender = renderMyAccountInfo(data);
            document.querySelector('.account-dynamic-data').innerHTML = toRender;
        })
        .catch(function () {
            document.querySelector(".error-message").classList.add("show");
        });
}

function setupAuthenticatedAccount() {
    getCurrentUser()
        .then(function (res) {
            if (!res.ok)
                throw res.statusText;

            return res.json();
        })
        .then(function (user) {
            //show user menu
            document.querySelector("a[href='#logged-account']").classList.remove("hide");
            document.querySelector("a[href='#signup']").classList.add("hide");
            document.querySelector("a[href='#signin']").classList.add("hide");
        })
        .catch(function () {
           

        });
}

function createTinyUrl() {
    var originalUrlValue = document.querySelector("#long-url").value;
    if (originalUrlValue == "") {
        event.preventDefault();
        event.stopPropagation();
        document.querySelector(".invalid-feedback").style.display = "block";
        return false;
    }
    var payload = {
        originalUrl: document.querySelector("#long-url").value,
        alias: document.querySelector("#alias").value
    };
    createUrl(payload)
        .then(function (res) { return res.json(); })
        .then(function (data) {
            document.querySelector(".create-container").parentElement.classList.add("after-create");

            document.querySelector("#visit-short-url").href = data.tinyUri
            document.querySelector("#tiny-url").value = data.tinyUri
            document.querySelector("#original-url").value = data.originalUrl;

        });
}

function editTinyUrl(currentEl) {

    currentEl.closest(".flex-fill").querySelector(".alias").classList.add("hide");
    currentEl.closest(".flex-fill").querySelector(".input-group.mb-3.hide").classList.remove("hide");
}

function undoEditChanges(currentEl) {
    currentEl.closest(".flex-fill").querySelector(".alias").classList.remove("hide");
    currentEl.closest(".flex-fill").querySelector(".input-group.mb-3").classList.add("hide");
}

function undoRenameChanges(currentEl) {
    currentEl.closest(".flex-fill").querySelector(".original-url.short-url-2").classList.remove("hide");
    currentEl.closest(".flex-fill").querySelector(".input-group.rename-original-url.mb-3").classList.add("hide");
}

function renameTinyUrl(currentEl) {
    currentEl.closest(".flex-fill").querySelector(".original-url.short-url-2").classList.add("hide");
    currentEl.closest(".flex-fill").querySelector(".input-group.rename-original-url.mb-3.hide").classList.remove("hide");
}

function pushEditChanges() {
    getByShortForm(document.querySelector("#collapseMyUrls").querySelector(".alias").innerText.trim())
        .then(function (res) {
            if (!res.ok)
                throw res.statusText;

            return res.json();
        })
        .then(function (tinyUri) {
            var payload = {
                id: tinyUri.id,
                newAlias : document.querySelector("#collapseMyUrls").querySelector("#alias").value,
                newOriginalUrl: document.querySelector("#collapseMyUrls").querySelector("#new_origina_Url").value
            }
            
            updateUrl(payload)
                .then(function (res) {
                    if (!res.ok)
                        throw res.statusText;
                    fetchAllUrls();
                })
            })
        .catch(function () {
            document.querySelector(".error-message").classList.add("show");
        });

    
}

function fetchByShortForm(shortUri) {
    getByShortForm(shortUri)
        .then(function (res) {
        if (!res.ok)
            throw res.statusText;

            return res.json();
        })
        .catch(function () {
            document.querySelector(".error-message").classList.add("show");
        });
}

function copyToClipboad() {
    var copyText = document.querySelector("#tiny-url");
    copyText.select();
    copyText.setSelectionRange(0, 99999);
    navigator.clipboard.writeText(copyText.value);

    var tooltip = document.querySelector("#copy-tooltip");
    tooltip.innerHTML = "Copied: " + copyText.value;
}

function showCopyTooltip() {
    var tooltip = document.querySelector("#copy-tooltip");
    tooltip.innerHTML = "Copy to clipboard";
}

function shortenAnother() {
    document.querySelector("#long-url").value = "";
    document.querySelector("#alias").value = "";

    document.querySelector(".create-container").parentElement.classList.remove("after-create");
}

function validateInputOnClick() {
    event.preventDefault();
    event.stopPropagation();
    document.querySelector(".invalid-feedback").style.display = "none";
}

function toggleContent() {
    var parentClassList = document.querySelector(".nav-pills").parentElement.classList;
    if (parentClassList.contains("buttom-rounded"))
        parentClassList.remove("buttom-rounded");
    else
        parentClassList.add("buttom-rounded");

    document.querySelectorAll(".collapse").forEach((element) => { element.classList.remove("show"); });
}


function openSignInForm() {
    toggleContent();
    document.querySelector("#signin.collapse").classList.add("show");
}

function openMyUrls() {
    toggleContent();
    document.querySelector("#collapseMyUrls.collapse").classList.add("show");
    fetchAllUrls();
}

function pushSignUpForm() {
    var username = document.querySelector("#signup-name").value;
    var email = document.querySelector("#signup-email").value;
    var password = document.querySelector("#signup-password").value;
    var repeatPass = document.querySelector("#signup-repeat-password").value;

    var terms = document.querySelector("#signup-terms").checked;

    if (username == "" || email == "" || password == "" || repeatPass == "" || terms == false) {
        event.preventDefault();
        event.stopPropagation();
        document.querySelector("#signup").querySelector(".invalid-feedback").classList.add("show");
        return false;
    }

    if (password != repeatPass) {
        event.preventDefault();
        event.stopPropagation();
        document.querySelector("#signup").querySelector(".invalid-feedback").classList.add("show");
        return false;
    }

    var payload = {
        email: email,
        password: password,
        username: username
    };
    register(payload)
        .then(function (res) {
            if (!res.ok)
                throw res.statusText;

        })
        .then(function () {
            document.querySelector("#signup").querySelector(".valid-signup-feedback").classList.add("show");
        })
        .catch(function (err) {
            document.querySelector("#signup").querySelector(".invalid-feedback").innerHTML = err;
            document.querySelector("#signup").querySelector(".invalid-feedback").classList.add("show");
        });

}

function openSignUpForm() {
    toggleContent();
    document.querySelector("#signup.collapse").classList.add("show");
}

function pushSignInForm(currentElement) {
    var email = document.querySelector("#signin-username").value;
    var password = document.querySelector("#signin-password").value;

    if (email == "" || password == "") {
        event.preventDefault();
        event.stopPropagation();
        document.querySelector(".invalid-feedback").style.display = "block";
        return false;
    }

    var payload = {
        email: email,
        password: password
    };
    login(payload)
        .then(function (res) {
            if (!res.ok)
                throw res.statusText;

        })
        .then(function () {
            currentElement.closest("#signin").querySelector(".invalid-feedback").classList.remove("show");
            toggleContent();
            setupAuthenticatedAccount();
            getCurrentUserInfo;
            document.querySelector("#logged-account.collapse").classList.add("show");

        })
        .catch(function (err) {
            currentElement.closest("#signin").querySelector(".invalid-feedback").innerHTML = err;
            currentElement.closest("#signin").querySelector(".invalid-feedback").classList.add("show");
        });
}


function fetchAllUrls() {
    document.querySelector(".spinner").classList.add("show");
    getRecentTinyUrls()
        .then(function (res) {
            if (!res.ok)
                throw res.statusText;

            return res.json();
        })
        .then(function (data) {
            if (data.length == 0) {
                document.querySelector(".spinner").classList.remove("show");
                document.querySelector(".error-message > p").innerHTML = "There are no tiny urls to show you!"
                document.querySelector(".error-message").classList.add("show");
                return;
            }

            var toRender = renderMyUrls(data);
            document.querySelector('#collapseMyUrls .border-top').innerHTML = toRender;
            document.querySelector(".spinner").classList.remove("show");
        })
        .catch(function () {
            document.querySelector(".spinner").classList.remove("show");
            document.querySelector(".error-message").classList.add("show");
    });

}
function renderMyUrls(data) {
    var view = {
        myTinyUrls: data,
        formatDate: function () {
            return function dateFormating(dateString, render) {
                var processedDate = render(dateString);
                const options = { year: "numeric", month: "long", day: "numeric", hour: "numeric" }
                return new Date(processedDate).toLocaleDateString(undefined, options)
            }
        }
    };


    var rendered = Mustache.render(
        `{{#myTinyUrls}}
                    <div>
                        <div class="url-list-item p-3 border-bottom">
                            <div>
                                <div class="d-flex">
                                    <div class="flex-fill">
                                        <div class="alias">
                                            <div class="text-break font-weight-semibold">
                                                {{tinyUri}}
                                            </div>
                                        </div>
                                        <div class="input-group mb-3 hide">
                                            <div class="input-group-prepend">
                                                <span class="input-group-text italic-font" id="basic-addon3">[ShortURLDomain]</span>
                                            </div>
                                            <input type="text" class="form-control" value={{alias}} id="alias" aria-describedby="basic-addon3">
                                            <div class="input-group-append">
                                                <button aria-label="Save changes" onClick="pushEditChanges()" class="btn btn-outline-t-green">
                                                    <i class="fa fa-check"></i>
                                                </button> 
                                                <button aria-label="Cancel editing" onClick="undoEditChanges(this)" class="btn btn-outline-danger rounded-right">
                                                    <i class="fas fa-times"></i>
                                                </button>
                                            </div>                                                  
                                        </div>
                                        <div class="input-group rename-original-url mb-3 hide">
                                            <input type="text" class="form-control" value={{originalUrl}} id="new_origina_Url" aria-describedby="basic-addon3">
                                            <div class="input-group-append">
                                                <button aria-label="Save changes" onClick="pushEditChanges()" class="btn btn-outline-t-green">
                                                    <i class="fa fa-check"></i>
                                                </button> 
                                                <button aria-label="Cancel renaming" onClick="undoRenameChanges(this)" class="btn btn-outline-danger rounded-right">
                                                    <i class="fas fa-times"></i>
                                                </button>
                                            </div>                                                  
                                        </div>
                                        <div class="original-url short-url-2 small flex-grow-1 text-t-lime-dark">
                                            {{originalUrl}}
                                        </div>
                                        <div class="d-flex justify-content-between flex-wrap">
                                            <div class="flex-fill align-self-center">
                                                <!----> <small class="text-muted text-right flex-shrink-0">
                                                    {{#formatDate}}{{createdAt}}{{/formatDate}}
                                                </small>
                                            </div>
                                            <div class="ml-auto">
                                               <a href="{{tinyUri}}" target="_blank" class="btn btn-sm btn-t-teal btn-outline-blue flex-fill small mr-1 mb-1 has-tooltip" data-original-title="null">
                                                    <i class="fa fa-share"></i>
                                               </a>
                                               <button onClick="renameTinyUrl(this)" class="btn btn-sm btn-t-teal text-nowrap mr-1 mb-1 opac has-tooltip" data-original-title="null">
                                               Rename
                                               </button>
                                               <button onClick="editTinyUrl(this)" class="btn btn-sm btn-t-teal text-nowrap mr-1 mb-1 opac has-tooltip" data-original-title="null">
                                               Edit
                                               </button>
                                               <button class="btn btn-sm btn-t-teal btn-success flex-fill text-nowrap mb-1 custom-tooltip" id="copy-btn" onclick="copyToClipboad()" type="button">
                                                    <i class="fa-regular fa-copy"></i>
                                                Copy
                                               </button> <!---->
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
            {{/myTinyUrls}}`, view);
    return rendered;
}
function renderMyAccountInfo(data) {
    var view = {
        account: data,
        formatDate: function () {
            return function dateFormating(dateString, render) {
                var processedDate = render(dateString);
                const options = { year: "numeric", month: "long", day: "numeric", hour: "numeric" }
                return new Date(processedDate).toLocaleDateString(undefined, options)
            }
        }
    };


    var rendered = Mustache.render(
        `<div class="row g-0">
                                            <div class="col-md-4 gradient-custom text-center"
                                                 style="border-top-left-radius: .5rem; border-bottom-left-radius: .5rem;">

                                                <svg class="img-fluid my-5" style="width: 80px;" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><!--! Font Awesome Pro 6.2.1 by @fontawesome - https://fontawesome.com License - https://fontawesome.com/license (Commercial License) Copyright 2022 Fonticons, Inc. --><path d="M256 112c-48.6 0-88 39.4-88 88C168 248.6 207.4 288 256 288s88-39.4 88-88C344 151.4 304.6 112 256 112zM256 240c-22.06 0-40-17.95-40-40C216 177.9 233.9 160 256 160s40 17.94 40 40C296 222.1 278.1 240 256 240zM256 0C114.6 0 0 114.6 0 256s114.6 256 256 256s256-114.6 256-256S397.4 0 256 0zM256 464c-46.73 0-89.76-15.68-124.5-41.79C148.8 389 182.4 368 220.2 368h71.69c37.75 0 71.31 21.01 88.68 54.21C345.8 448.3 302.7 464 256 464zM416.2 388.5C389.2 346.3 343.2 320 291.8 320H220.2c-51.36 0-97.35 26.25-124.4 68.48C65.96 352.5 48 306.3 48 256c0-114.7 93.31-208 208-208s208 93.31 208 208C464 306.3 446 352.5 416.2 388.5z" /></svg>
                                                <h5>{{account.userName}}</h5>
                                                <div>
                                                    <a href="#!" onclick="logOutUser()" class="btn btn-blue">Log Out</a>
                                                </div>
                                            </div>
                                            <div class="col-md-8">
                                                <div class=" p-4">
                                                    <h6>Information</h6>
                                                    <hr class="mt-0 mb-4">
                                                    <div class="row pt-1">
                                                        <div class="col-12 mb-3">
                                                            <div> Email:  
                                                            <p class="inline text-muted">{{account.email}}</p>
                                                            </div>
                                                            <div>Joined:
                                                            <p class="inline text-muted">{{#formatDate}}{{account.createdAt}}{{/formatDate}}</p>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    
                                                </div>
                                            </div>
                                        </div>`, view);
    return rendered;
}