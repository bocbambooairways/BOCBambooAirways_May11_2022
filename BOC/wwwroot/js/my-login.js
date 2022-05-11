$(function () {
    if ($("#error").val() !== "Model.ErrorMessage" && $("#error").val() !== "") {
        alertify.alert($("#error").val());
        return false;
    }
    if ($("#result").val() !== "") {
        $('#password').val("");

    }
});