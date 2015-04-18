$(function () {

    $("#frmLogin").validate({
        ignore: "",
        rules: {
            txtEmailID: {
                required: true,
                email: true
            },
            txtPassword: {
                required: true
            }
        }, //rules

        errorPlacement: function (error, element) {
            if (element.attr("name") == "txtEmailID")
                error.appendTo("#errEmail");
            if (element.attr("name") == "txtPassword")
                error.appendTo("#errPwd");
        },
        submitHandler: function (form) {

        },

        invalidHandler: function (form, validator) {

        }
    }); //validate()

});   //doc ready

function fnVal() {
    $("#divLoginErr").html("");

    if ($("#frmLogin").valid()) {
        $("#btnMwsLogin").button('loading');
        return true;
    }
    else {
        return false;
    }
}
