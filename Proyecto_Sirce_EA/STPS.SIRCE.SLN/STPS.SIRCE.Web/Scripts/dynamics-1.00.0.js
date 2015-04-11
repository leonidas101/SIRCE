//#region Credits
/**
* dynamics.js v1.0.0 by @cibarra
* Copyright 2015 Code Retailers.
* Regions works with Web Essentials
* http://www.apache.org/licenses/LICENSE-2.0
*/
//#endregion

//#region Dynamic Spinner
/**
*  ╔╦╗┬ ┬┌┐┌┌─┐┌┬┐┬┌─┐┌─┐  ╔═╗┌─┐┬┌┐┌┌┐┌┌─┐┬─┐
*   ║║└┬┘│││├─┤│││││  └─┐  ╚═╗├─┘│││││││├┤ ├┬┘
*  ═╩╝ ┴ ┘└┘┴ ┴┴ ┴┴└─┘└─┘  ╚═╝┴  ┴┘└┘┘└┘└─┘┴└─
* Función utilizada para cargar el spinner de Dynamics.
* Parametros: 
* elementID - El ID del div donde se empotrara el Spinner.
* loaderText - Texto descriptivo que se mostrara con el loader.
*/
(function ( $ ) {
    $.fn.dynamicSpinner = function (options) {
        var settings = $.extend({
            // These are the defaults.
            loadingText: "Cargando...",
            elementID: $("body")
        }, options);
        $('<div id="screenBlock"></div>').appendTo(settings.elementID);
        $('#screenBlock').css({ opacity: 0, width: $(settings.elementID).width() * 1.1, height: $(settings.elementID).height()* 10000 });
        $('#screenBlock').addClass('blockDiv');
        $('#screenBlock').animate({ opacity: 0.7 }, 200);
        $('body').addClass('stop-scrolling');
        var html = "<div class='spinner'><img src='../Image/spinner.gif' />";
        html += "<h2>" + settings.loadingText + "</h2></div>";
        var width = ($(settings.elementID).width() / 2.18);
        var height = ($(settings.elementID).height() / 3.5);
        $(html).appendTo('#screenBlock');
        $(".spinner").css({ "margin-left": width });
        $(".spinner").css({ "margin-top": height });
    };
}(jQuery));
(function ( $ ) {
    $.fn.dynamicSpinnerDestroy = function (options) {
        $('body').removeClass('stop-scrolling');
        $('#screenBlock').animate({ opacity: 0 }, 200, function () {
            $('#screenBlock').remove();
        });
    };
}(jQuery));
//#endregion

//#region Dynamic Panels
/**
*  ╔╦╗┬ ┬┌┐┌┌─┐┌┬┐┬┌─┐┌─┐  ╔═╗┌─┐┌┐┌┌─┐┬  ┌─┐
*   ║║└┬┘│││├─┤│││││  └─┐  ╠═╝├─┤│││├┤ │  └─┐
*  ═╩╝ ┴ ┘└┘┴ ┴┴ ┴┴└─┘└─┘  ╩  ┴ ┴┘└┘└─┘┴─┘└─┘
* Función utilizada para generar los Paneles Dynamics.
* Parametros tomados de los atritubutos de los divs: 
* titulo - El ID del div donde se empotrara el Spinner.
* icono - Texto descriptivo que se mostrara con el loader.
*/
(function ($) {
    $.fn.dynamicPanels = function (options) {
        var html = '<div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">';
        $(this).each(function (key, value) {
            html += '<div class="panel panel-default">';
            html += '<div class="panel-heading" role="tab" id="' + $(value).attr("id") + 'Titulo">';
            html += '<h4 class="panel-title">';
            html += '<i class="fa fa-' + $(value).attr("icono") + '"></i>';
            if (key == 0)
                html += '<a data-toggle="collapse" data-parent="#accordion" href="#' + $(value).attr("id") + '" aria-expanded="true" aria-controls="' + $(value).attr("id") + '">';
            else
                html += '<a class="collapsed" data-toggle="collapse" data-parent="#accordion" href="#' + $(value).attr("id") + '" aria-expanded="false" aria-controls="' + $(value).attr("id") + '">';
            html += $(value).attr("titulo")
            html += '</a></h4></div>';
            if (key == 0)
                html += ' <div id="' + $(value).attr("id") + '" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="' + $(value).attr("id") + 'Titulo">';
            else
                html += ' <div id="' + $(value).attr("id") + '" class="panel-collapse collapse" role="tabpanel" aria-labelledby="' + $(value).attr("id") + 'Titulo">';
            html += '<div class="panel-body"></div></div></div>'
        });
        html += '</div>';
        $("#contenido").html(html);
    };
}(jQuery));
//#endregion

//#region Dynamic DropDown
/**
*  ╔╦╗┬ ┬┌┐┌┌─┐┌┬┐┬┌─┐┌─┐  ╔╦╗┬─┐┌─┐┌─┐╔╦╗┌─┐┬ ┬┌┐┌
*   ║║└┬┘│││├─┤│││││  └─┐   ║║├┬┘│ │├─┘ ║║│ │││││││
*  ═╩╝ ┴ ┘└┘┴ ┴┴ ┴┴└─┘└─┘  ═╩╝┴└─└─┘┴  ═╩╝└─┘└┴┘┘└┘
* Función utilizada para generar los DropDown Dynamics (Seleccion simple o múltiple).
* Parametros tomados de los atritubutos de los divs: 
* titulo - El ID del div donde se empotrara el Spinner.
* icono - Texto descriptivo que se mostrara con el loader.
*/
(function ($) {
    $.fn.dynamicDropDown = function (options) {
        var id = this.selector;
        var settings = $.extend({
            // These are the defaults.
            placeholder: "-Seleccionar-",
            id: "catalogoID",
            desc: "catalogoDescripcion"
        }, options);
        $(id).append("<option value='0'></option>")
        $(settings.data).each(function (key, value) {
            $(id).append('<option value=' + value[settings.id] + '>' + value[settings.desc] + '</option>')
        });
        $(id).chosen({
            no_results_text: "¡No se encontraron coincidencias!",
            allow_single_deselect: true,
            width: "74%"
        })
        $(id).parent().height("58px");
    };
}(jQuery));

(function ($) {
    $.fn.dynamicDropDownDestroy = function (options) {
        var id = this.selector;
        $(id).empty();
        $(id).chosen('destroy');
    };
}(jQuery));
//#endregion

//#region Dynamic Table
/**
*  ╔╦╗┬ ┬┌┐┌┌─┐┌┬┐┬┌─┐┌─┐  ╔╦╗┌─┐┌┐ ┬  ┌─┐
*   ║║└┬┘│││├─┤│││││  └─┐   ║ ├─┤├┴┐│  ├┤ 
*  ═╩╝ ┴ ┘└┘┴ ┴┴ ┴┴└─┘└─┘   ╩ ┴ ┴└─┘┴─┘└─┘
* Función utilizada para cargar la tabla de Dynamics.
* Parametros: 
* elementID - El ID del div donde se empotrara el Spinner.
* loaderText - Texto descriptivo que se mostrara con el loader.
*/
(function ($) {
    $.fn.dynamicTable = function (options) {
        var id = this.selector;
        var settings = $.extend({
            // These are the defaults.
            emptyText: "No se obtubieron resultados",
        }, options);
        //Se agregan los botonoes a la lista
        var html = "<div class='col-lg-12'><div class='btn-group' role='group'>";
        $(settings.data.iconosGrid).each(function (key, value) {
            html += " <button type='button' class='btn btn-info icon-grid' onclick='" + String(value["callback"]) + "($(this))' ";
            if (!value["enabled"])
                html += " disabled "
            html += " data-toggle='tooltip' data-placement='bottom' title='" + value["tooltip"] + "' ";
            html += " accion= '" + value["accion"] + "' " + "defaultEnabled=" + value["enabled"];
            html += "><i class='fa fa-" + value["icono"] + "'></i></button>"
        });
        html += "</div></div>"
        //Se crean los encabezados de la tabla
        html += "<table id='"+id.substring(1)+"Table' class='table table-striped table-hover table-condensed' border ='1'><thead><tr>";
        $(settings.data.encabezados).each(function (key, value) {
            html += "<th>" + value + "</th>";
        });
        html += "</tr></thead><tbody>";
        //Se crean las columnas de la tabbla
        $(settings.data.datos).each(function (key, value) {
            html +="<tr value='"+value[settings.data.columnaID]+"'>";
            $(settings.data.columnas).each(function (key2, value2) {
                html += '<td>' + value[value2] + '</td>';
            });
            html += "</tr>";
        });
        html += "</tbody></table>"
        $(id).html(html);
        $($(id + "Table").selector).DataTable({
            "aLengthMenu": [[20, 50, 100, -1], [20, 50, 100, "*"]],
            "oLanguage": {
                "sProcessing": "Procesando...",
                "sLengthMenu": "Mostrar _MENU_ registros",
                "sZeroRecords": "No se encontraron resultados",
                "sEmptyTable": "Ningún dato disponible en esta tabla",
                "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                "sInfoPostFix": "",
                "sSearch": "Buscar:",
                "sUrl": "",
                "sInfoThousands": ",",
                "sLoadingRecords": "Cargando...",
                "oPaginate": {
                    "sFirst": "Primero",
                    "sLast": "Último",
                    "sNext": "Siguiente",
                    "sPrevious": "Anterior"
                },
                "oAria": {
                    "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                    "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                }
            }
        });
        $(id + " tbody").on('click', 'tr', function () {
            debugger;
            $("tbody tr").removeClass("active");
            $(this).addClass("active");
            if ($(".active").attr('value') != undefined) {
                $("button").removeAttr("disabled");
                var html = '';
                if (settings.data.datos[0].acciones != null) {
                    var selected = jQuery.grep(settings.data.datos, function (obj) {
                        return obj.listaID == $(".active").attr('value');
                    });
                    $(selected[0].acciones).each(function (key, value) {
                        html += " <button type='button' class='btn btn-info icon-grid' onclick='" + String(value["callback"]) + "($(this))' ";
                        if (!value["enabled"])
                            html += " disabled "
                        html += " data-toggle='tooltip' data-placement='bottom' title='" + value["tooltip"] + "' ";
                        html += " accion= '" + value["accion"] + "' " + "defaultEnabled=" + value["enabled"];
                        html += "><i class='fa fa-" + value["icono"] + "'></i></button>"
                    });
                    $(this).parent().parent().parent().parent().find('.btn-group').html(html);
                }
            }
        });

        $('[data-toggle="tooltip"]').tooltip();

        $(id).on('page.dt', function () {
            $($("button")).each(function (key, value) {
                if(!$(value).attr("defaultEnabled"))
                    $("button").attr("disabled", "disabled");
            });
            $("tbody tr").removeClass("active");
            
        });
    };
}(jQuery));
//#endregion

//#region Dynamic Calendar
/**
*  ╔╦╗┬ ┬┌┐┌┌─┐┌┬┐┬┌─┐┌─┐  ╔═╗┌─┐┬  ┌─┐┌┐┌┌┬┐┌─┐┬─┐
*   ║║└┬┘│││├─┤│││││  └─┐  ║  ├─┤│  ├┤ │││ ││├─┤├┬┘
*  ═╩╝ ┴ ┘└┘┴ ┴┴ ┴┴└─┘└─┘  ╚═╝┴ ┴┴─┘└─┘┘└┘─┴┘┴ ┴┴└─
* Función utilizada para cargar los calendarios de Dynamics.
* Parametros: 
* elementID - El ID del div donde se empotrara el Spinner.
* loaderText - Texto descriptivo que se mostrara con el loader.
*/
(function ($) {
    $.fn.dynamicCalendar = function (options) {
        var id = this.selector;
        var settings = $.extend({
            // These are the defaults.
            textoCalendario: "Seleccionar una Fecha",
        }, options);
        $(id).datepicker({
            showOn: "button",
            buttonImage: "../Image/calendar.png",
            buttonImageOnly: true,
            changeMonth: true,
            changeYear: true,
            showAnim: "slideDown",
            buttonText: settings.textoCalendario
        });
        /* Inicialización en español para la extensión 'UI date picker' para jQuery. */
        /* Traducido por Vester (xvester@gmail.com). */
        (function (factory) {
            if (typeof define === "function" && define.amd) {

                // AMD. Register as an anonymous module.
                define(["../datepicker"], factory);
            } else {

                // Browser globals
                factory(jQuery.datepicker);
            }
        }(function (datepicker) {

            datepicker.regional['es'] = {
                closeText: 'Cerrar',
                prevText: '&#x3C;Ant',
                nextText: 'Sig&#x3E;',
                currentText: 'Hoy',
                monthNames: ['enero', 'febrero', 'marzo', 'abril', 'mayo', 'junio',
                'julio', 'agosto', 'septiembre', 'octubre', 'noviembre', 'diciembre'],
                monthNamesShort: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
                'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                dayNames: ['domingo', 'lunes', 'martes', 'miércoles', 'jueves', 'viernes', 'sábado'],
                dayNamesShort: ['dom', 'lun', 'mar', 'mié', 'jue', 'vie', 'sáb'],
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                weekHeader: 'Sm',
                dateFormat: 'dd/mm/yy',
                firstDay: 1,
                isRTL: false,
                showMonthAfterYear: false,
                yearSuffix: ''
            };
            datepicker.setDefaults(datepicker.regional['es']);

            return datepicker.regional['es'];

        }));
    };
}(jQuery));
//#endregion

//#region Dynamic Lock
/**
*  ╔╦╗┬ ┬┌┐┌┌─┐┌┬┐┬┌─┐┌─┐  ╦  ┌─┐┌─┐┬┌─
*   ║║└┬┘│││├─┤│││││  └─┐  ║  │ ││  ├┴┐
*  ═╩╝ ┴ ┘└┘┴ ┴┴ ┴┴└─┘└─┘  ╩═╝└─┘└─┘┴ ┴
* Función utilizada para bloquear todos los elementos de un Form Seleccionado.
*/
(function ($) {
    $.fn.dynamicLock = function (options) {
        //Deshabilita Inputs 
        var id = this.selector;
        debugger;
        $('input, select, button ', id).each(function (key, value) {
            var elemento = this;
            $(this).attr("disabled", "disabled");
            $(this).datepicker('disable');
            $(this).trigger("chosen:updated");
        });
    };
    $.fn.dynamicLockDestroy = function (options) {
        //Deshabilita Inputs 
        var id = this.selector;
        debugger;
        $('input, select, button ', id).each(function (key, value) {
            var elemento = this;
            $(this).prop("disabled", false);
            $(this).datepicker("option", "disabled", false);
            $(this).trigger("chosen:updated");
        });
    };
}(jQuery));
//#endregion*

//#region Dynamic Modal
/**
*  ╔╦╗┬ ┬┌┐┌┌─┐┌┬┐┬┌─┐┌─┐  ╔╦╗┌─┐┌┬┐┌─┐┬  
*   ║║└┬┘│││├─┤│││││  └─┐  ║║║│ │ ││├─┤│  
*  ═╩╝ ┴ ┘└┘┴ ┴┴ ┴┴└─┘└─┘  ╩ ╩└─┘─┴┘┴ ┴┴─┘
* Función utilizada para cargar el modal Dynamic.
* Parametros: 
* elementID - El ID del div donde se empotrara el Spinner.
* loaderText - Texto descriptivo que se mostrara con el loader.
*/
(function ($) {
    $.fn.dynamicModal = function (options) {
        debugger;
        var id = this.selector;
        var settings = $.extend({
            // These are the defaults.
            titulo: "Seleccionar:",
        }, options);
        //Se agregan los botonoes a la lista
        var html = "<!-- Modal --><div class='modal fade' id='"+ id.substring(1) +"Modal' tabindex='-1' role='dialog' aria-labelledby='myModalLabel' data-backdrop='static' data-keyboard='false' aria-hidden='true'>";
        html += "<div class='modal-dialog modal-lg'><div class='modal-content'><div class='modal-header'>";
        html += "<h4 class='modal-title'>" + settings.titulo + "</h4></div></div></div></div>";
        $(id).html(html);
        $.ajax({
            url: settings.url,
            type: 'POST',
            data: settings.data,
            //Mostrar Dynamic Loader
            beforeSend: function () {
                $("body").dynamicSpinner({
                    loadingText: "Cargando..."
                });
            },
            success: function (result) {
                //Remover Dynamic Loader
                $("body").dynamicSpinnerDestroy({});
                $(id + "Modal .modal-content").append(result);
            }
        });
        $(id + "Modal").modal('show');
    };
}(jQuery));
//#endregion