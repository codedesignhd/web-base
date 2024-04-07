var formLogin = document.getElementById("formLogin")
var formRegister = document.getElementById("formRegister")

formLogin.addEventListener("submit", async function (event) {
    event.preventDefault()
    var emailInput = document.getElementById("email")
    var passwordInput = document.getElementById("password")
    var rememberCkb = document.getElementById("remember-me")

    const formData = new FormData()
    formData.set("username", emailInput.value)
    formData.set("password", passwordInput.value)
    formData.set("is_remember", rememberCkb.checked)

    const response = await fetch('/PostLogin', {
        method: "POST",
        body: formData,
    })
    const data = await response.json()
    if (data.success) {
        location.href = data.data.redirectUri
    }
    else {
        alert(data.message)
    }
    if (response.ok) {

    }
})

formRegister.addEventListener("submit", async function (event) {
    event.preventDefault()
    var usernameInput = document.getElementById("username")
    var emailInput = document.getElementById("email")
    var passwordInput = document.getElementById("password")
    var termsConditionCkb = document.getElementById("terms-conditions")


    const formData = new FormData()
    formData.set("username", usernameInput.value)
    formData.set("email", emailInput.value)
    formData.set("password", passwordInput.value)

    const response = await fetch('/PostRegister', {
        method: "POST",
        body: formData,
    })
    const data = await response.json()
    if (data.success) {
        location.href = data.data.redirectUri
    }
    else {
        alert(data.message)
    }
    if (response.ok) {

    }
})


