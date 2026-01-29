//Button Loading
$.fn.buttonLoader = function (action) {
    var self = $(this);

    var butonid = $(this).attr('id');

    if (action == 'start') {
        if ($(self).attr("disabled") == "disabled") {
            e.preventDefault();
        }
        $("#" + butonid).attr("disabled", "disabled");
        $(self).attr('data-btn-text', $(self).text() + "<i class='mdi mdi-mail ms-2'></i>");
        $(self).html('<span class="spinner"><i class="fa fa-spinner fa-spin"></i></span>Lütfen Bekleyin');
        $(self).addClass('active');
    }
    if (action == 'stop') {
        $(self).html($(self).attr('data-btn-text'));
        $(self).removeClass('active');
        $("#" + butonid).removeAttr("disabled");
    }
};
//Button Loading



//Toast Mesaj 
function toastmesaj(uyaritipi, mesaj, baslik,) {

    Command: toastr[uyaritipi](mesaj, baslik)
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": 300,
        "hideDuration": 1000,
        "timeOut": 5000,
        "extendedTimeOut": 1000,
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"

    }
}
//Toast Mesaj 

//Proforma Fatura Mail Gönder
$(".ProformaPdfMailGonder").click(function () {
    var btn = $(this);

    var siparisid = $(this).attr("data-siparisid");

    $(btn).buttonLoader('start');

    $.ajax({
        url: '/Admin/Siparisler/ProformaFaturaMailGonder/' + siparisid,
        dataType: "json",
        type: "POST",
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        success: function (result) {
            if (result.Basarilimi == true) {

                toastmesaj(result.MesajDurumu, result.Mesaj, "Başarılı");
            }
            else {
                toastmesaj(result.MesajDurumu, result.Mesaj, "Hata Oluştu");
                $(btn).buttonLoader('stop');

            }
        },
        error: function (result) {
            toastmesaj(result.MesajDurumu, result.Mesaj, "Genel Bir Hata Oluştu");
            $(btn).buttonLoader('stop');

        },
        beforeSend: function () {
        },
        complete: function () {
            $(btn).buttonLoader('stop');
        }
    });
});

//Proforma Fatura Mail Gönder




//Modal
function AdminModalAc() {
    $(".admin-modal-ac").click(function () {

        $('.modal-title').text($(this).data("modalbaslik"));
        var url = $(this).data("target");

        var urlpost = $(this).data("urlpost");
        var formid = $(this).data("formid");
        var sayfayenile = $(".admin-modal-ac").data("sayfayenile");

        var entityid = $(this).data("entityid");


        $.get(url, function (data) { })
            .done(function (data) {
                $('.modal').on('shown.bs.modal', function () {
                    $('#EntityId').val(entityid);
                    $(".modal-content .modal-footer #btnGonder").data("btnurlpost", urlpost);
                    $(".modal-content .modal-footer #btnGonder").data("btnformid", formid);
                    $(".modal-content .modal-footer #btnGonder").data("btnsayfayenile", sayfayenile);
                    $('.modal-icerik form').data('id', formid);
                });

                $("#modal .modal-icerik").html(data);
                $("#modal").modal("show");

                $('.modal-icerik form').attr('id', formid);
                $(".modal-content .modal-footer #btnGonder").attr("btnurlpost", urlpost);
                $(".modal-content .modal-footer #btnGonder").attr("btnformid", formid);
                $(".modal-content .modal-footer #btnGonder").attr("btnsayfayenile", sayfayenile);
            })
            .fail(function () {
                $("#modal .modal-icerik").text("Hata Oluştu!!");
                $("#modal").modal("show");
            })
    });

    $('#btnGonder').click(function () {

        var btnurlpost = $(this).data("btnurlpost");
        var formid = $(this).data("btnformid");
        var sayfayenile = $(this).data("btnsayfayenile");
        var formdata = new FormData($("#" + formid)[0]);

        var btn = $(this);
        $(btn).buttonLoader('start');
        $.ajax({
            type: 'POST',
            url: btnurlpost,
            data: formdata,
            /*data: $("#" + formid).serialize(),*/
            contentType: false,
            processData: false,
            enctype: 'multipart/form-data',
            success: function (result) {

                if (result.Basarilimi == true) {
                    toastmesaj(result.MesajDurumu, result.Mesaj, "Başarılı");
                    $("#" + formid)[0].reset();
                    $("#" + formid + " input[type='text']").val("");
                }
                else {
                    toastmesaj(result.MesajDurumu, result.Mesaj, "Hata Oluştu");
                    $(btn).buttonLoader('stop');

                }

            },
            error: function (result) {
                toastmesaj(result.MesajDurumu, result.Mesaj, "Genel Bir Hata Oluştu");
                $(btn).buttonLoader('stop');

            },
            beforeSend: function () {
            },
            complete: function () {
                $(btn).buttonLoader('stop');
            }
        });

    });
}
AdminModalAc();
//Modal