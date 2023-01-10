window.addEventListener('DOMContentLoaded', (event) => {
    setEventListeners();
});

function setEventListeners() {
    var myUrlsSection = document.querySelector('#collapseMyUrls')
    myUrlsSection.addEventListener('show.bs.collapse', fetchAllUrls);
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
            currentElement.closest("#signin").querySelector(".valid-signup-feedback").classList.add("show");
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
                                        <div class="short-url-2 small flex-grow-1 text-t-lime-dark">
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
                                               <button class="btn btn-sm btn-t-teal text-nowrap mr-1 mb-1 opac has-tooltip" data-original-title="null">
                                               Rename
                                               </button>
                                               <button class="btn btn-sm btn-t-teal text-nowrap mr-1 mb-1 opac has-tooltip" data-original-title="null">
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