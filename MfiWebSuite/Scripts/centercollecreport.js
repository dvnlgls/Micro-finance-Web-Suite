
$(function () {

    $("#drpCenter").change(function () {
        var CenterID = $("#drpCenter option:selected").val();
        $('#uxMeetingDate').html('');

        if (CenterID != "0") {
            $('#uxMeetingDate').attr('disabled', 'disabled');
            fnGetMeetingDate(CenterID);
        }
    });


    $("#drpCenter option:eq(0)").attr('selected', true);

    $("#btnShowRepaymentForm").click(function () {
        if (!$(this).hasClass('disabled')) {

            $('#uxRepBody').html("");

            var repDt = $("#uxMeetingDate option:selected").val();

            if (repDt) {
                var CenterID = $("#drpCenter option:selected").val();

                $(this).addClass('disabled');
                fnGetRepData(CenterID, repDt);

            }
            else {
                alert("Please select a meeting date");
            }
        }
    });

});         //doc ready

function fnGetRepData(centerID, plannedDate) {
    $.ajax({
        type: "POST",
        url: "/Ajax/loan.aspx",
        data: "AjaxMethod=GetRepaymentData" +
                    "&CenterID=" + centerID +
                    "&PlannedDate=" + plannedDate +
                    "&IsReport=1",
        success: function (returnResponse) {
            var element = jQuery(returnResponse).find("#hdnLUK");

            if (element.length == 1) {
                window.location = "/login?rr=se";
            }
            else {
                $('#btnShowRepaymentForm').removeClass('disabled');
                if (returnResponse != 'Error_Default')
                    $('#uxRepBody').html(returnResponse);
            }
        },
        error: function (xhr) {
            $('#btnShowRepaymentForm').removeClass('disabled');
        }
    });
}

function fnGetMeetingDate(centerID) {
    $.ajax({
        type: "POST",
        url: "/Ajax/loan.aspx",
        data: "AjaxMethod=GetMeetingDatesForCenter" +
                    "&CenterID=" + centerID,
        success: function (returnResponse) {
            var element = jQuery(returnResponse).find("#hdnLUK");

            if (element.length == 1) {
                window.location = "/login?rr=se";
            }
            else {
                $('#uxMeetingDate').removeAttr('disabled');
                if (returnResponse != 'Error_Default')
                    $('#uxMeetingDate').html(returnResponse);

            }
        },
        error: function (xhr) {
            $('#uxMeetingDate').removeAttr('disabled');
        }
    });
}

