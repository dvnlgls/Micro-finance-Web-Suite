$(function () {

    $("#uxBranch").keypress(function () {
        $(this).trigger("change");
    });

    $("#uxVillage").keypress(function () {
        $(this).trigger("change");
    });

    $("#uxCenter").keypress(function () {
        $(this).trigger("change");
    });

    $("#uxBranch").change(function () {

        var BranchID = $("#uxBranch option:selected").val();

        $("#uxVillage option").show();
        $("#uxVillage option:eq(0)").attr('selected', true);

        $("#uxFE option").show();
        $("#uxFE option:eq(0)").attr('selected', true);

        if (BranchID != "0") {
            $("#uxVillage option").show().not("." + BranchID).hide();
            $("#uxVillage option:eq(0)").show().attr('selected', true);

            $("#uxFE option").show().not("." + BranchID).hide();
            $("#uxFE option:eq(0)").show().attr('selected', true);
        }

        $("#uxVillage").trigger("change");
    });



    $("#uxVillage").change(function () {

        var VillageID = $("#uxVillage option:selected").val();

        $("#uxCenter option").show();
        $("#uxCenter option:eq(0)").attr('selected', true);

        if (VillageID != "0") {

            $("#uxCenter option").show().not("." + VillageID).hide();
            $("#uxCenter option:eq(0)").show().attr('selected', true);
        }

        //$("#uxCenter").trigger("change");

    });


    $("#uxAsOnDate").datepicker({
        dateFormat: 'd M y',
        changeYear: true,
        changeMonth: true,
        defaultDate: null,
        maxDate: 0,
        showButtonPanel: true
    });

    $("#uxFromDate").datepicker({
        dateFormat: 'd M y',
        changeYear: true,
        changeMonth: true,
        maxDate: 0,
        showButtonPanel: true,
        onSelect: function (selectedDate) {
            $("#uxToDate").datepicker("option", "minDate", selectedDate);
        }
    });

    $("#uxToDate").datepicker({
        dateFormat: 'd M y',
        changeYear: true,
        changeMonth: true,
        maxDate: 0,
        showButtonPanel: true,
        onSelect: function (selectedDate) {
            $("#uxFromDate").datepicker("option", "maxDate", selectedDate);
        }
    });

    $('#uxMonthPicker').monthpicker({
        pattern: 'mm-yyyy' //don't change the pattern. else date can't be retireved. use val() to get date
    });


    $("#btnShowReport").click(function () {

        if (!$("#btnShowReport").hasClass('disabled')) {

            $("#btnShowReport").addClass('disabled')

            var RID = $("#ContentPlaceHolder1_hdnRID").val();

            var BranchID = null;
            try {
                BranchID = $("#uxBranch option:selected").val();
            } catch (e) { }

            var VillageID = null;
            try {
                VillageID = $("#uxVillage option:selected").val();
            } catch (e) { }

            var CenterID = null;
            try {
                CenterID = $("#uxCenter option:selected").val();
            } catch (e) { }

            var FeID = null;
            try {
                FeID = $("#uxFE option:selected").val();
            } catch (e) { }

            var LpID = null;
            try {
                LpID = $("#uxLP option:selected").val();
            } catch (e) { }

            var AsOnDt = null;
            try {
                AsOnDt = $("#uxAsOnDate").datepicker("getDate");
                AsOnDt = AsOnDt.getFullYear() + "-" + (AsOnDt.getMonth() + 1) + "-" + AsOnDt.getDate();
            } catch (e) { }

            var FromDt = null;
            try {
                FromDt = $("#uxFromDate").datepicker("getDate");
                FromDt = FromDt.getFullYear() + "-" + (FromDt.getMonth() + 1) + "-" + FromDt.getDate();
            } catch (e) { }

            var ToDt = null;
            try {
                ToDt = $("#uxToDate").datepicker("getDate");
                ToDt = ToDt.getFullYear() + "-" + (ToDt.getMonth() + 1) + "-" + ToDt.getDate();
            } catch (e) { }

            var Month = null;
            var Year = null;
            try {
                Month = $('#uxMonthPicker').val().split('-')[0];
                Year = $('#uxMonthPicker').val().split('-')[1];
            } catch (e) { }


            $.ajax({
                type: "POST",
                url: "/Ajax/reports.aspx",
                data: "AjaxMethod=GetReport" +
                    "&RID=" + RID +
                    "&BranchID=" + BranchID +
                    "&VillageID=" + VillageID +
                    "&CenterID=" + CenterID +
                    "&FeID=" + FeID +
                    "&LpID=" + LpID +
                    "&AsOnDt=" + AsOnDt +
                    "&FromDt=" + FromDt +
                    "&ToDt=" + ToDt +
                    "&Month=" + Month +
                    "&Year=" + Year,

                success: function (returnResponse) {
                    $("#locNotHO").html("");

                    var element = jQuery(returnResponse).find("#hdnLUK");

                    if (element.length == 1) {
                        window.location = "/login?rr=se";
                    }
                    else {
                        $("#btnShowReport").removeClass('disabled')

                        if (returnResponse == "Error_Default") {

                            $("#locNotHO").html("<div id='frmAlert' class='alert alert-block alert-error'></div>");
                            $("#frmAlert").append("Sorry, encountered an unexpected error. Please try again.");
                        }
                        else {
                            $("#uxReportTblWrap").html(returnResponse);
                        }
                    }
                }, //success
                error: function (xhr) {
                    $("#btnShowReport").removeClass('disabled')

                    $("#locNotHO").html("<div id='frmAlert' class='alert alert-block alert-error'></div>");
                    $("#frmAlert").append("Sorry, encountered an unexpected error. Please try again.");

                }
            });      //ajax
        }

    });

});