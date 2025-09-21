
$(document).ready(function () {

    $(function () {

        $("[id*=ShowHideEmployeeDashBoardFormsImageButton]").each(function () {
            if ($(this)[0].src.indexOf("minus") != -1) {
                $(this).closest("tr").after("<tr><td></td><td colspan='999'>" + $(this).next().html() + "</td></tr>")
                $(this).next().remove();
            }
        });

    });

    // Fixes the space on the first th of the grid for collapse and expand
    $("[id*=EmployeeInterventionsGridView]").find('th:first').css("width", "1%");

});
