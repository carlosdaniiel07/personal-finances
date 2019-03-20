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

    // DataTable with export data buttons
    $('.datatable-export').DataTable({
        "scrollX": true,
        "dom": 'Bfrtip',
        "pageLength": 50,
        buttons: [
            {
                extend: 'excelHtml5',
                className: 'btn btn-success btn-flat btn-export-data',
                text: 'Export to Excel',
                exportOptions: {
                    modifier: {
                        page: 'current'
                    }
                }
            },
            {
                extend: 'csvHtml5',
                className: 'btn btn-info btn-flat btn-export-data',
                text: 'Export to CSV',
                exportOptions: {
                    modifier: {
                        page: 'current'
                    }
                }
            },
            {
                extend: 'pdfHtml5',
                className: 'btn btn-danger btn-flat btn-export-data',
                text: 'Export to PDF',
                exportOptions: {
                    modifier: {
                        page: 'current'
                    }
                }
            }
        ]
    });

    // jQuery Mask
    $('.money').mask("###0,00", { reverse: true });

    // Fix sidebar white space at bottom of page on resize
    $(window).on("load", function () {
        setTimeout(function () {
            $("body").layout("fix");
            $("body").layout("fixSidebar");
        }, 250);
    });
});