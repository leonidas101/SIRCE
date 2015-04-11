/**
*  Site Javascript by @cibarra
*/

//#region Login Navbar Function
function setUser(userName) {
    $(userName).appendTo('#user');
}

$('body').on('keyup', ':input', function (e) {
    $(this).val($(this).val().toUpperCase());
});
//#endregion