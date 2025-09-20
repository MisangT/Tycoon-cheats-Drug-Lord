
$(document).ready(function () {
    type = "text/javascript"; src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js";

    $("[src*=plus]").live("click", function () {
        $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
        $(this).attr("src", "../../../../_layouts/16/FIC.LD.TrainingEvaluation.SP.Solution/images/minus.png");
    });
$("[src*=minus]").live("click", function () {
    $(this).attr("src", "../../../../_layouts/16/FIC.LD.TrainingEvaluation.SP.Solution/images/plus.png");
    $(this).closest("tr").next().remove();
});

});