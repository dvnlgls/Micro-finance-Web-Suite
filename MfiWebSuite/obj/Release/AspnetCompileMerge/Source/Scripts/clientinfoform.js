$(function () {
    $("#ContentPlaceHolder1_hdnOID").val($("#drpVil option:selected").attr("id"));

    $("#drpVil").change(function () {
        $("#ContentPlaceHolder1_hdnOID").val($("#drpVil option:selected").attr("id"));
    });

    $("#form1").validate({
        ignore: "",
        rules: {
            ctl00$ContentPlaceHolder1$uxClientName: {
                required: true,
                maxlength: 50
            }
        }, //rules

        errorPlacement: function (error, element) {
            if (element.attr("name") == "ctl00$ContentPlaceHolder1$uxClientName")
                error.appendTo("#errClientName");

        },
        submitHandler: function (form) {

        },

        invalidHandler: function (form, validator) {

        }
    }); //validate()

});

function fnVal() {
    if (!$("#ContentPlaceHolder1_btnSave").hasClass('disabled')) {

        if ($("#form1").valid()) {
            $("#ContentPlaceHolder1_btnSave").addClass("disabled");
            return true;
        }
        else {
            return false;
        }
    }
}