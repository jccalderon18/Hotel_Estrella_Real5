<script type="text/javascript">
    $(document).ready(function () {
        // Configuración para DataTable con idioma en Español (Colombia)
        var languageSettings = {
        "sEmptyTable": "No hay datos disponibles en la tabla",
    "sInfo": "Mostrando _START_ a _END_ de _TOTAL_ entradas",
    "sInfoEmpty": "Mostrando 0 a 0 de 0 entradas",
    "sInfoFiltered": "(filtrado de _MAX_ entradas en total)",
    "sLengthMenu": "Mostrar _MENU_ entradas",
    "sLoadingRecords": "Cargando...",
    "sProcessing": "Procesando...",
    "sSearch": "Buscar:",
    "sZeroRecords": "No se han encontrado resultados",
    "oPaginate": {
        "sFirst": "Primero",
    "sLast": "Último",
    "sNext": "Siguiente",
    "sPrevious": "Anterior"
            },
    "oAria": {
        "sSortAscending": ": activar para ordenar la columna de manera ascendente",
    "sSortDescending": ": activar para ordenar la columna de manera descendente"
            }
        };

    // Default Datatable
    $('#datatable').DataTable({
        language: languageSettings  // Añadimos la configuración de idioma aquí
        });

    // Buttons examples
    var table = $('#datatable-buttons').DataTable({
        lengthChange: false,
    buttons: [
    {
        extend: 'excel',
    text: '<i class="bi bi-filetype-exe"></i>',
    className: 'botonexcel btn',
    titleAttr: 'exportar excel'
                },
    {
        extend: 'pdf',
    text: '<i class="bi bi-filetype-pdf"></i>',
    className: 'botonpdf btn',
    titleAttr: 'exportar pdf'
                }
    ],
    scrollX: true,
    language: languageSettings,  // Añadimos la configuración de idioma aquí también
    initcomplete: function (json, settings) {
        // Eliminar la clase de los estilos por defecto de DataTables
        $(".dt-buttons").removeClass("dt-buttons");
    // Agregar clase para añadir estilos personalizados
    $(".dt-button").addClass("botones");
            }
        });

    // Key Tables
    $('#key-table').DataTable({
        keys: true,
    language: languageSettings  // Configurar idioma también aquí
        });

    // Responsive Datatable
    $('#responsive-datatable').DataTable({
        language: languageSettings  // Configurar idioma también aquí
        });

    // Multi Selection Datatable
    $('#selection-datatable').DataTable({
        select: {
        style: 'multi'
            },
    language: languageSettings  // Configurar idioma también aquí
        });

    // Mover los botones de exportación al contenedor adecuado
    table.buttons().container()
    .appendTo('#datatable-buttons_wrapper .col-md-6:eq(0)');
    });
</script>