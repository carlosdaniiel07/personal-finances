$(function () {
    // Datepicker
    $(".datepicker").datepicker({
        autoclose: true,
        format: "dd/mm/yyyy",
        todayBtn: "linked",
        todayHighlight: true
    });

    // DataTable
    $('.datatable').DataTable({
        "scrollX": true
    });

    // jQuery Mask
    $('.money').mask("###0,00", { reverse: true });

    // iCheck
    $('input[type="checkbox"], input[type="radio"]').iCheck({
        checkboxClass: "icheckbox_minimal-blue",
        radioClass: "iradio_minimal-blue"
    });

    // Fix sidebar white space at bottom of page on resize
    $(window).on("load", function () {
        setTimeout(function () {
            $("body").layout("fix");
            $("body").layout("fixSidebar");
        }, 250);
    });
});