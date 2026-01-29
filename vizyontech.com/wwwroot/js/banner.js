$(document).ready(function () {
    function handleMenuTipiChange(element) {
        var myID = $(element).val(); // Seçilen MenuTipi ID'sini al

        console.log("Seçilen MenuTipi ID:", myID);

        // Hangi 'tr' satırında çalışıyoruz?
        var $currentRow = $(element).closest('tr'); // Şu anki satırı al
        var trId = $currentRow.attr('id'); // TR ID'si


        var idParts = trId.split('-'); // "banner-row-{DilId}-{BannerRow}" formatında
        var dilId = idParts[2]; // DilId
        var bannerRow = idParts[3]; // BannerRow

        // Dosya Secim işlemleri için kontrol
        var $dosyaSecim = $(`#DosyaSecim-${dilId}-${bannerRow}`); // Benzersiz DosyaSecim ID
        var $dosyaListele = $(`#DosyaListele-${dilId}-${bannerRow}`); // Benzersiz DosyaListele ID

        var $dosyaSecim = $(`#DosyaSecim-${dilId}-${bannerRow}`); // Benzersiz DosyaSecim ID
        var $dosyaListele = $(`#DosyaListele-${dilId}-${bannerRow}`); // Benzersiz DosyaListele ID

        if (myID === "31") {


            DosyaKategori();

            var initialSelectedValue = $(`#EntityId-${dilId}-${bannerRow}`).val();

            if (initialSelectedValue) {
                $dosyaListele.val(initialSelectedValue).trigger('change');

            }


            $dosyaListele.off('change').on('change', function () {
                var selectedValue = $(this).val();

                $(`#EntityId-${dilId}-${bannerRow}`).val(selectedValue); // EntityId'yi güncelle
                $dosyaListele.select2('destroy').select2();


            });




        } else {
            $dosyaSecim.css("display", "none");
            $(`#EntityId-${dilId}-${bannerRow}`).val('');
        }



        // Tüm 'sayfatipi' div'lerini kontrol et
        $currentRow.find('.sayfatipi').each(function () {
            var sayfaTipiID = $(this).attr('id');
            var parsedID = sayfaTipiID.split('-')[2]; // ID'nin son kısmını al

            if (parsedID === myID) {
                $(this).show(); // Eşleşen div'i göster

                // İçindeki select'in change olayını bağla
                var $select = $(this).find('select');
                var $entityInput = $(`#EntityId-${dilId}-${bannerRow}`);

                // Başlangıçta seçili değeri input'a ata
                var initialSelectedValue = $select.val();
                $entityInput.val(initialSelectedValue);




                // Değer değiştiğinde güncelle
                if (!$select.data('change-bound')) {
                    $select.change(function () {
                        var selectedValue = $(this).val(); // Seçilen değeri al
                        $entityInput.val(selectedValue); // EntityId'yi güncelle

                        // selectedValue ve myID'yi sayıya dönüştürerek karşılaştırma yapıyoruz
                        if (Number(myID) === 31 && Number(selectedValue) > 0) {
                            $dosyaSecim.css("display", "block");
                            console.log("DosyaSecim Görüntülendi");
                        } else {
                            $dosyaSecim.css("display", "none"); // Eğer şart sağlanmazsa gizle
                            console.log("DosyaSecim Gizlendi");
                        }
                    });
                    $select.data('change-bound', true); // Olayın bağlandığını işaretle

                    if (myID == 31 && initialSelectedValue != "0") {
                        $dosyaSecim.css("display", "block");
                    } 

                }

                if (myID === "31") {

                    var $dosyaListeleSelect = $(`#DosyaListele-${dilId}-${bannerRow}`); // Benzersiz ID ile select'i seç
                    var dosyaListeleValue = $dosyaListeleSelect.val(); // Seçili değeri al
                    console.log(`Dosya Listele Value (Dil ${dilId}, Satır ${bannerRow}):`, dosyaListeleValue);

                    // Seçili değeri başlangıçta EntityId input'a ata
                    if (dosyaListeleValue && dosyaListeleValue !== "0" && $entityInput.val() !== dosyaListeleValue) {
                        $entityInput.val(dosyaListeleValue);
                        console.log(`EntityId Başlangıçta Güncellendi (Dil ${dilId}, Satır ${bannerRow}): ${dosyaListeleValue}`);

                    }

                    // Liste değiştirildiğinde değerleri güncelle
                    $dosyaListeleSelect.off('change').on('change', function () {


                        var updatedValue = $(this).val(); // Güncellenen değeri al
                        if (updatedValue && updatedValue !== "0") {
                            $entityInput.val(updatedValue); // EntityId'yi güncelle
                            console.log(`EntityId Güncellendi (Dosya Listeleme Değişimi, Dil ${dilId}, Satır ${bannerRow}):`, updatedValue);

                            // **UI Güncellemesi:**
                            $dosyaListeleSelect.val(updatedValue).trigger('change.select2'); // Select2'yi güncelle
                        } else {
                            // Eğer "Seçiniz" seçildiyse
                            $entityInput.val(''); // EntityId'yi sıfırla
                            console.log(`EntityId temizlendi (Dosya Listeleme Değişimi, Dil ${dilId}, Satır ${bannerRow})`);
                        }
                    });

                    // **Başlangıç UI Güncellemesi**
                    if ($dosyaListeleSelect.hasClass('select2-hidden-accessible')) {
                        $dosyaListeleSelect.select2('destroy').select2(); // Select2'yi yeniden başlat
                    }
                } else {
                    console.log("myID 31 değil, diğer işlemler devam ediyor.");
                }





            } else {
                $(this).hide(); // Eşleşmeyen div'leri gizle
                $(this).find('select').val('0'); // Varsayılan değere sıfırla
            }
        });
    }

    // Sayfa yüklendiğinde ve değişiklik durumunda işlemleri bağla
    $(document).ready(function () {
        $('[id^="seourl-"]').change(function () {
            handleMenuTipiChange(this);
        });

        $('[id^="seourl-"]').each(function () {
            handleMenuTipiChange(this);
        });
    });




    //Dosya Seçimi
    //$('.dosyakategoriid').change(function () {
    //    var dosyaListele = $("#DosyaListele");
    //    var secinizOption = dosyaListele.find('option:first').val();
    //    $("#DosyaListele").val(secinizOption);

    //    $("#DosyaSecim").css("display", "block");
    //});


    function DosyaKategori() {

        //$("#DosyaListele").val("0").trigger("change");

        $.ajax({
            type: 'POST',
            url: '/Admin/Menuler/DosyalariGetir',
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            data: { id: $(".dosyakategoriid").val() },
            success: function (data) {
                $("#DosyaListele").prop("disabled", false);
                var seciliDeger = $("#DosyaListele").val(); // Seçili değeri sakla
                $("#DosyaListele").empty(); // Mevcut seçenekleri temizle
                $("#DosyaListele").append('<option value="">Seçiniz</option>');

                $.each(data, function (i, dosya) {
                    $("#DosyaListele").append('<option value="'
                        + dosya.Id + '">'
                        + dosya.Resim + '</option>');
                });
                $("#DosyaListele").val(seciliDeger);

            },
            error: function (ex) {
                alert('Hata Oluştu' + ex.responseText);
            }
        });
    };
    //Dosya Seçimi

});