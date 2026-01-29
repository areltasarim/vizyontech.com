

function HeaderSepet() {
    $.ajax({
        url: '/Sepet/_HeaderSepet/',
        type: "get",
        success: function (result) {
            $(".headersepetListesi").html(result);
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



function Sepet() {
    $.ajax({
        url: '/Sepet/_Sepet/',
        type: "get",
        success: function (result) {
            $("#sepetListesi").html(result);
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

function SepetMobil() {
    $.ajax({
        url: '/Sepet/_SepetMobil/',
        type: "get",
        success: function (result) {
            $("#sepetListesiMobil").html(result);
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


function SepetUrunSayisi() {
    $.ajax({
        url: '/Sepet/_SepetUrunSayisi/',
        type: "get",
        success: function (result) {
            $(".sepeturunsayisi").html(result);
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

function SepetGenelToplam() {
    $.ajax({
        url: '/Sepet/_SepetGenelToplam/',
        type: "get",
        success: function (result) {
            $("#sepetGenelToplam").html(result);
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

function SepetGenelToplamTekSayfaOdeme() {
    $.ajax({
        url: '/Sepet/_SepetGenelToplamTekSayfaOdeme/',
        type: "get",
        success: function (result) {
            $("#sepetGenelToplamTekSayfaOdeme").html(result);
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
function AlisverisListeUrunSayisi() {
    $.ajax({
        url: '/Sepet/_AlisverisListeUrunSayisi/',
        type: "get",
        success: function (result) {
            $(".alisverislisteurunsayisi").html(result);
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
AlisverisListeUrunSayisi();


function SepeteEkle(urunid, adet, sepetguncellemDurum) {


    function getSeciliUrunSecenekleri() {
        var seciliUrunSecenekleri = Array.from(document.querySelectorAll('.UrunSecenekDeger a.active'))
            .map(x => x.getAttribute("data-secenekdegerid"));


        return seciliUrunSecenekleri;
    }

    var seciliUrunSecenekleriArray = getSeciliUrunSecenekleri();
  
    //if (seciliUrunSecenekleriArray.includes("")) {
    //    alert("Lütfen Ürün Değerlerini Seçiniz");
    //    return false;
    //}

    var PostData = {
        UrunId: urunid,
        Adet: adet,
        UrunSecenekleri: seciliUrunSecenekleriArray.toString(),
        SepetAdetGuncellemeDurum: sepetguncellemDurum,
    };

    $.ajax({
        url: '/Sepet/AddToCart',
        data: PostData,
        dataType: "json",
        type: "POST",
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        success: function (result) {

            if (result.Basarilimi == true) {
                HeaderSepet();
                Sepet();
                SepetUrunSayisi();
                SepetGenelToplam();
                toastmesaj(result.MesajDurumu, result.Mesaj, result.Display);
            }
            else {
                toastmesaj(result.MesajDurumu, result.Mesaj, result.Display);
            }
        },
        error: function (result) {
            toastmesaj(result.MesajDurumu, result.Mesaj, result.Display);
        },
        beforeSend: function () {
        },
        complete: function () {

        }
    });
}

function SepetUrunSil(UrunId) {

    $.ajax({
        url: '/Sepet/RemoveCart?UrunId=' + UrunId,
        data: { UrunId: UrunId },
        dataType: "json",
        type: "POST",
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        success: function (result) {
            if (result.Basarilimi == true) {

                Sepet();
                SepetMobil();
                HeaderSepet();
                SepetUrunSayisi();
                SepetGenelToplam();
                SepetGenelToplamTekSayfaOdeme();
                toastmesaj(result.MesajDurumu, result.Mesaj, result.Display);
            }
            else {
                toastmesaj(result.MesajDurumu, result.Mesaj, result.Display);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(textStatus);
        },
        beforeSend: function () {
        },
        complete: function () {

        }
    });

}



function SepetModal(UrunId) {

    $.ajax({
        url: '/Sepet/_SepetModal',
        type: 'GET',
        data: { urunId: UrunId },
        success: function (response) {
            var modal = $('#sepete_modal');
            modal.find('#sepet_modal_content').html(response);
            modal.modal('show');
            HeaderSepet();
            SepetUrunSayisi();
        },
        error: function (xhr, status, error) {
            console.error('Hata Oluştu: ', error);
        },
        beforeSend: function () {
        },
        complete: function () {

        }
    });
}


function AlisverisListemeEkle(UrunId) {
    var PostData = {
        UrunId: UrunId,
    };
    $.ajax({
        url: '/Sepet/AlisverisListemeEkle',
        data: PostData,
        dataType: "json",
        type: "POST",
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        success: function (result) {
            AlisverisListeUrunSayisi();
            toastmesaj(result.MesajDurumu, result.Mesaj, result.Display);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(textStatus);
        },
        beforeSend: function () {
        },
        complete: function () {

        }
    });


}

function AlisverisListemUrunSil(UrunId) {

    $.ajax({
        url: '/Account/AlisverisListeUrunSil?UrunId=' + UrunId,
        data: { UrunId: UrunId },
        dataType: "json",
        type: "POST",
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        success: function (result) {
            if (result.Basarilimi == true) {
                $('#alisverislistem-' + UrunId).remove();
                AlisverisListeUrunSayisi();

            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(textStatus);
        },
        beforeSend: function () {
        },
        complete: function () {

        }
    });

}

function AlisverisListesiModal(UrunId) {
    $.ajax({
        url: '/Sepet/_AlisverisListemModal',
        type: 'GET',
        data: { urunId: UrunId },
        success: function (response) {
            var modal = $('#alisverisliste_modal');
            modal.find('#alisverisliste_modal_content').html(response);
            modal.modal('show');
        },
        error: function (xhr, status, error) {
            console.error('Hata Oluştu: ', error);
        },
        beforeSend: function () {
        },
        complete: function () {

        }
    });
}

function BegeniModal(UrunId) {
    $.ajax({
        url: '/Sepet/_BegeniModal',
        type: 'GET',
        data: { urunId: UrunId },
        success: function (response) {
            var modal = $('#begeni_modal');
            modal.find('#begeni_modal_content').html(response);
            modal.modal('show');

        },
        error: function (xhr, status, error) {
            console.error('Hata Oluştu: ', error);
        },
        beforeSend: function () {
        },
        complete: function () {

        }
    });
}

function HizliGoruntuleModal(UrunId) {
    $.ajax({
        url: '/Urunler/_HizliGoruntule',
        type: 'GET',
        data: { urunId: UrunId },
        success: function (response) {
            var modal = $('#hizligoruntule_modal');
            modal.find('#hizligoruntule_modal_content').html(response);
            modal.modal('show');
        },
        error: function (xhr, status, error) {
            console.error('Hata Oluştu: ', error);
        },
        beforeSend: function () {
        },
        complete: function () {

        }
    });
}

$('#btnKupon').on('click', function (e) {
    e.preventDefault(); // Sayfanın yenilenmesini engeller
    var kuponKodu = $('input[name="KuponKodu"]').val();
    $.ajax({
        url: '/Sepet/KuponKullan',
        type: 'POST',
        dataType: "json",
        data: {
            KuponKodu: kuponKodu,
        },
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        success: function (result) {
            toastmesaj(result.MesajDurumu, result.Mesaj, result.Display);
            SepetGenelToplam();
            SepetGenelToplamTekSayfaOdeme();

        },
        error: function (error) {
            toastmesaj(result.MesajDurumu, result.Mesaj, result.Display);
        }
    });
});
