
$(document).ready(function () {

    $('input[id$=tbDateFrom]').datepicker({});
    $('input[id$=tbDateTo]').datepicker({});

    // Reject Section - disable button if reason is blank - enable button if reason is populated

    $(function () {
        $('#RejectInterventionButton').attr('disabled', 'disabled');
    });


    $('#ctl00_ctl39_g_a70cd799_99f1_4b99_ad7e_d287fabf0908_ctl00_RejectReasonTextbox').keyup(function () {

        var reason = $('#ctl00_ctl39_g_a70cd799_99f1_4b99_ad7e_d287fabf0908_ctl00_RejectReasonTextbox').val();

        if (reason.trim().length <= 0)
            $('#RejectInterventionButton').attr('disabled', 'disabled');
        else
            $('#RejectInterventionButton').removeAttr('disabled');

    });

    // Load the Rejects pop up window
    $('input[id$=ShowRejectButton]').click(function () {

        $("#RejectDiv").dialog({

            title: "Reject Intervention",

            width: 1055,

            height: 270,

            modal: true,

            resizable: false,

            buttons: [{
                id: "RejectInterventionButton",
                text: "Reject",
                click: function () {
                    $('#RejectInterventionButton').attr('disabled', 'disabled');
                    var reason = $('#ctl00_ctl39_g_a70cd799_99f1_4b99_ad7e_d287fabf0908_ctl00_RejectReasonTextbox').val();
                    if (reason.trim().length <= 0)
                        alert('Reject Reason can not be empty.')
                    else {
                        var InterventionID = $('input[id$=InterventionIDHidden]').val()
                        window.location.href = "/CS/HR/LD/SitePages/apply.aspx?i=" + InterventionID + "&r=" + reason;
                    }
                }
            },
            {
                id: "CancelInterventionButton",
                text: "Cancel",
                click: function () {
                    $(this).dialog("close");
                }
            }]
        });

        return false;
    });

    $(function () {
        $("#dialog").dialog({
            modal: true,
            autoOpen: false,
            title: "Escalate Intervention",
            width: 1000,
            height: 150,
        });
        $("#btnShowEscalationPopup").click(function () {
            $('#dialog').dialog('open');
        });
    });

    $('input[id$=lnkBtnEscaInter]').click(function () {
        function SetTextAreaValue() {
            var name = $get("txtEscalationReason").value;

            $("input:text#ReasonHidden").val(name);
            $('input[name=ReasonHidden]').val(name);

            $(this).dialog("close");


        }
    });

    var count = 1;
    $('input[id$=addRowButton]').click(function () {

        var InitialObjectiveTextBox = '<input type="text" name="InitialObjectiveTextBox' + count + '" id="InitialObjectiveTextBox' + count + '" class="inputTextBox" required>'
        var ActionPlanTextBox = '<input type="text" name="ActionPlanTextBox' + count + '" id="ActionPlanTextBox' + count + '" class="inputTextBox" required>'
        var HowToImplementTextBox = '<input type="text" name="HowToImplementTextBox' + count + '" id="HowToImplementTextBox' + count + '" class="inputTextBox" required>'
        var WhenItShouldBeDoneTextBox = '<input type="text" name="WhenItShouldBeDoneTextBox' + count + '" id="WhenItShouldBeDoneTextBox' + count + '" class="inputTextBox" required>'

        var row = "row_" + count

        if (count > 4) {
            alert("Only 5 rows allowed");
            return false;
        }

        $("#ActionPlanTable").append("<tr id='" + row + "'><td>" + InitialObjectiveTextBox + "</td><td>" + ActionPlanTextBox + "</td><td>" + HowToImplementTextBox + "</td><td>" + WhenItShouldBeDoneTextBox + "</td></tr>");
        $("#ActionPlanTable").append("<tr id='" + row + "'><td>&nbsp;</td></tr>");

        $('input[id$=RowsAddedHidden]').val(count);
        count++;
    });

    $('input[id$=removeRowButton]').click(function () {
        if (count == 1) {
            alert("No rows available to remove");
            return false;
        }

        count--;

        $('input[id$=RowsAddedHidden]').val(count);

        $("#row_" + count).remove();
        $("#row_" + count).remove();

    });

    // Check the TD with SetStatusColorRed css class
    $("td.SetStatusColorRed").each(function () {
        var notCompleted = $(this).text();

        if (notCompleted == "Completed") {
            $("td.SetStatusColorRed").addClass('SetStatusColorGreen');
            $("td.SetStatusColorRed").removeClass('SetStatusColorRed');
        }
    });

    $(function () {

        $("[id*=ShowHideEmployeeDashBoardFormsImageButton]").each(function () {
            if ($(this)[0].src.indexOf("minus") != -1) {
                $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
                $(this).next().remove();
            }
        });

    });
});
