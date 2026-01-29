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


//İl Seçimi
function Adres() {
    $("#FaturaTuru").change(function () {
        if (this.checked) {
            $("#FaturaTuru").val("Kurumsal");
        }
        else {
            $("#FaturaTuru").val("Bireysel");
        }
    });


    $('.adres_ulke').change(function () {

        var ilId = $(this).data('il');

        var selectedDropdownId = $(this).attr('id');

        var selectedDropdownValue = $('#' + selectedDropdownId).val();

        $("#kasabilgitopla").click(function () {
            if ($("#AdresEkle_IlId :selected").val() == 0) {
                alert("İl alanı boş bırakılamaz..!");
                return false;
            }
        });

        $("#" + ilId).empty();



        $.ajax({
            type: 'POST',
            url: '/Account/IlleriGetir',
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            data: { id: selectedDropdownValue },
            success: function (data) {
           
                $("#" + ilId).append("<option value='0'>Seçiniz...</option>");

                $("#" + ilId).prop("disabled", false);
       
                $.each(data, function (i, il) {
                    $("#" + ilId).append('<option value="'
                        + il.Id + '">'
                        + il.IlAdi + '</option>');
                });
            },
            error: function (ex) {
                alert('Hata Oluştu' + ex.responseText);
            }
        });
        return false;

    });

    $(".adres_il").change(function () {
        var ilceId = $(this).data('ilce'); // örneğin: AdresEkle_IlceId
        var selectedIlId = $(this).val();  // seçilen il ID'si

        var $ilceSelect = $("#" + ilceId);
        $ilceSelect.empty();
        $ilceSelect.append("<option value=''>Seçiniz...</option>"); // value="0" değil, boş string

        $.ajax({
            type: 'POST',
            url: '/Account/IlceleriGetir',
            dataType: 'json',
            data: { id: selectedIlId },
            success: function (data) {
                $.each(data, function (i, state) {
                    $ilceSelect.append('<option value="' + state.Id + '">' + state.IlceAdi + '</option>');
                });

                $ilceSelect.prop("disabled", false);

                // 🔁 validation'ı tetikle
                $ilceSelect.removeClass("input-validation-error"); // önce varsa hata class'ı sil
            },
            error: function (ex) {
                alert('Hata Oluştu: ' + ex.responseText);
            }
        });
    });

}

Adres();

$(document).ready(function () {
    $('.adres').change(function () {//Clicking input radio
        var radioClicked = $(this).attr('id');
        unclickRadio();
        removeActive();
        clickRadio(radioClicked);
        makeActive(radioClicked);
    });
    $(".card").click(function () {//Clicking the card
        var inputElement = $(this).find('input[type=radio]').attr('id');
        unclickRadio();
        removeActive();
        makeActive(inputElement);
        clickRadio(inputElement);
    });
});


function unclickRadio() {
    $(".adres").prop("checked", false);
}

function clickRadio(inputElement) {
    $("#" + inputElement).prop("checked", true);
}

function removeActive() {
    $(".card").removeClass("active");
}

function makeActive(element) {
    $("#" + element + "-card").addClass("active");
}



function AdresSelectListe() {
    $.ajax({
        url: '/Account/_AdresSelectListe/',
        type: "get",
        success: function (result) {

            $(".AdresSelectListe").html(result);
        },
        error: function (jqXHR, textStatus, errorThrown) {

        },
        beforeSend: function () {
        },
        complete: function () {

        }
    });
}

const kasa = window.location.pathname;
if (kasa === '/kasa') {
    AdresSelectListe();
}



function AdresListesi() {
    $.ajax({
        url: '/Account/_AdresListesi/',
        type: "get",
        success: function (result) {
            $(".AdresListe").html(result);

            ModalAc();
            AjaxKaydet();
        },
        error: function (jqXHR, textStatus, errorThrown) {

        },
        beforeSend: function () {
        },
        complete: function () {

        }
    });
}


$("#btnSiparisiTamamla").click(function () {
    var teslimatAdresi = $('#TeslimatAdresId').find(":selected").val();
    var faturaAdresi = $('#FaturaAdresId').find(":selected").val();

    if (teslimatAdresi == 0) {
        alert("Lütfen bir teslimat adresi seçiniz.");
        return false;
    }
    else if (faturaAdresi == "0") {
        alert("Lütfen bir fatura adresi seçiniz.");
        return false;
    }
});


function AltKullaniciListe() {
    $.ajax({
        url: '/Account/_AltKullaniciListe/',
        type: "get",
        success: function (result) {
            $("#AltKullaniciListe").html(result);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Hata Oluştu");
        },
        beforeSend: function () {
        },
        complete: function () {

        }
    });
}


const fullUrl = window.location.href;
const baseUrl = `${window.location.protocol}//${window.location.host}`;
const relativePath = fullUrl.replace(baseUrl, '');

if (relativePath === '/adreslerim' || relativePath === '/hesabim?tip=adres') {
    AdresListesi();
}

if (relativePath === '/Account/AltKullaniciListe') {
    AltKullaniciListe();
}

if (relativePath === '/siparisler') {
    AjaxKaydet();

}


//Toast Mesaj 
function toastmesaj(uyaritipi, baslik, aciklama) {
    $.toast({
        heading: baslik,
        text: aciklama,
        icon: uyaritipi,
        position: 'top-right',
        loader: true,        // Change it to false to disable loader
        loaderBg: '#9EC600'  // To change the background
    });
}
//Toast Mesaj 


//Modal
function ModalAc() {

    $(".modal-ac").click(function () {

        $('.modal-title').text($(this).data("modalbaslik"));
        var url = $(this).data("target");

        var urlpost = $(this).data("urlpost");
        var formid = $(this).data("formid");
        var sayfayenile = $(".modal-ac").data("sayfayenile");

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

                Adres();

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

        if ($("#" + formid).valid()) {

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



                    if (formid == "frmAdresEkle") {
                        AdresListesi();
                    }
                    if (formid == "frmAdres") {
                        AdresSelectListe();

                    }
                    if (formid == "frmAdresGuncelle") {
                        AdresListesi();

                    }
                    // Formu temizle
                    $("#" + formid)[0].reset();
                    $("#" + formid + " input[type='text']").val("");

                    $("#modalmesaj").remove();


                    $(btn).buttonLoader('stop');


                    //eğer result da html elementleri var sa
                    if (typeof result === "string" && result.indexOf("</div>") !== -1) {
                        // ActionResult bir PartialViewResult döndürdü
                        $('#modal .modal-icerik').html(result);
                        $('.modal-icerik form').attr('id', formid);
                    } else if (typeof result === "object") {
                        AlertMessage(result.Basarilimi, result.Mesaj, result.MesajDurumu, sayfayenile)
                    }

                },
                error: function (xhr, status, error) {
                    var message = "";
                    if (!xhr.responseText) {
                        message = "İşlem sırasında bir hata oluştu. Lütfen tekrar deneyin";
                    } else {
                        message = xhr.responseText;
                    }
                    //console.log("error", xhr.responseText, typeof xhr.responseText, JSON.parse(xhr.responseText), xhr.responseText.Basarilimi, xhr.responseText.Mesaj, xhr.responseText.MesajDurumu);

                    var result = JSON.parse(xhr.responseText);

                    $(btn).buttonLoader('stop');


                    AlertMessage(result.Basarilimi, result.Mesaj, result.MesajDurumu, sayfayenile)
                }
            });
        }

    });

    $('#modal').on('hidden.bs.modal', function () {
        $('#response-status').empty(); // veya html("") kullanarak içeriğini silebilirsiniz.
    });

    function AlertMessage(Basarilimi = "", Message = "", MesajDurumu = "", sayfayenile = "") {
        $("#response-status").html('<div class="alert ' + MesajDurumu + ' fade show" role="alert">' + Message + '</div>');
        if (sayfayenile == true) {
            window.setTimeout(function () {
                location.reload();
            });
        }
    }
}
ModalAc();
//Modal


//Ajax Kaydet

var process = true;
function AjaxKaydet() {

    $('.ajaxKaydet').click(function () {

        if (process) {
            process = false;
            var btnurlpost = $(this).data("urlpost");
            var formid = $(this).data("formid")
            var formdata = new FormData($("#" + formid)[0]);
            var formreset = $(this).data("formreset")
            if ($("#" + formid).valid()) {
                var btn = $(this);
                $(btn).buttonLoader('start');
                $.ajax({
                    type: 'POST',
                    url: btnurlpost,
                    data: formdata,
                    processData: false,
                    contentType: false,
                    success: function (result) {

                        if (result.Basarilimi == true) {

                            toastmesaj(result.MesajDurumu, result.Mesaj, "", result.Display, 3000, "", "");
                            if (formreset == true) {
                                $("#" + formid)[0].reset();
                            }
                            if (formid == "frmAdresSil") {
                                AdresListesi();
                            }

                            if (formid == "frmAdres") {
                                AdresSelectListe();
                            }


                            if (formid == "frmSiparisGuncelle") {
                                Sepet();
                            }

                        } else {
                            toastmesaj(result.MesajDurumu, result.Mesaj, "", result.Display, 3000, "", "");
                        }

                        $(btn).buttonLoader('stop');
                        process = true;
                    },
                    error: function (result) {

                        toastmesaj(result.MesajDurumu, result.Mesaj, "", result.Display, 3000, "", "");
                        $(btn).buttonLoader('stop');
                        process = true;
                    },
                    beforeSend: function () {
                    },
                    complete: function () {

                    }
                });
            }
        }
    });
}

AjaxKaydet();
//Ajax Kaydet
