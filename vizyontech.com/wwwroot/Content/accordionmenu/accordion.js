// jQuery yüklendikten sonra çalış
(function() {
    function initAccordionMenu() {
        const offcanvasMenu = document.getElementById("offcanvas-menu");
        const openMenuButton = document.querySelector(".open-menu-button");
        const openMenuButtonMobile = document.getElementById("open-menu");
        const closeMenuButton = document.querySelector(".close-button");
        const closeMenuButtonMobile = document.getElementById("close-menu");
        const psMenuSlidebar = document.querySelector(".ps-menu--slidebar");
        const backdrop = document.getElementById("backdrop");

        if (!offcanvasMenu) return;

        // Menü açma fonksiyonu
        function openMenu() {
            offcanvasMenu.style.transform = "translateX(0)";
            backdrop.style.display = "block";
            if (psMenuSlidebar) {
                psMenuSlidebar.style.display = "none";
            }
            // Mobil menü butonunun parent'ına active class ekle
            if (openMenuButtonMobile && openMenuButtonMobile.parentElement) {
                openMenuButtonMobile.parentElement.classList.add("active");
            }
        }

        // Menü kapama fonksiyonu
        function closeMenu() {
            offcanvasMenu.style.transform = "translateX(-100%)";
            backdrop.style.display = "none";
            // Mobil menü butonunun parent'ından active class'ı kaldır
            if (openMenuButtonMobile && openMenuButtonMobile.parentElement) {
                openMenuButtonMobile.parentElement.classList.remove("active");
            }
        }

        // jQuery ile eski event listener'ları kaldır ve yeni event listener'ları ekle
        if (typeof jQuery !== 'undefined') {
            // Eski event listener'ları kaldır
            jQuery('#open-menu').off('click');
            jQuery('#close-menu').off('click');
            
            // Yeni event listener'ları ekle
            jQuery('#open-menu').on('click', function(e) {
                e.preventDefault();
                e.stopPropagation();
                openMenu();
                return false;
            });
            
            jQuery('#close-menu').on('click', function(e) {
                e.preventDefault();
                e.stopPropagation();
                closeMenu();
                return false;
            });
        }

        // Desktop menü açma butonu
        if (openMenuButton) {
            openMenuButton.addEventListener("click", (e) => {
                e.preventDefault();
                openMenu();
            });
        }

        // Desktop menü kapama butonu
        if (closeMenuButton) {
            closeMenuButton.addEventListener("click", (e) => {
                e.preventDefault();
                e.stopPropagation();
                closeMenu();
            });
        }

        // Backdrop'a tıklayınca menüyü kapat
        if (backdrop) {
            backdrop.addEventListener("click", () => {
                closeMenu();
            });
        }

        // Kategori item tıklama olaylarını dinle
        const categoryItems = document.querySelectorAll(".category-item");
        const sectionsContainer = document.getElementById("sections-container");

        categoryItems.forEach(item => {
            item.addEventListener("click", function(e) {
                e.preventDefault();
                
                const categoryId = this.getAttribute("data-category-id");
                
                // Aktif kategoriyi güncelle
                categoryItems.forEach(i => i.classList.remove("active"));
                this.classList.add("active");
                
                // İlgili kategoriyi göster
                showCategory(categoryId);
            });
        });

        // Kategori içeriğini göster
        function showCategory(categoryId) {
            if (!window.categoryData || !window.categoryData[categoryId]) {
                return;
            }

            const category = window.categoryData[categoryId];
            if (!sectionsContainer) return;

            sectionsContainer.innerHTML = '';

            if (category.sections && category.sections.length > 0) {
                category.sections.forEach((section, sectionIndex) => {
                    // Section group oluştur (2. seviye alt kategori başlığı)
                    const sectionGroup = document.createElement("div");
                    sectionGroup.className = "category-section-group";
                    sectionGroup.style.animationDelay = `${sectionIndex * 0.1}s`;

                    // Başlık oluştur
                    const sectionTitle = document.createElement("h3");
                    sectionTitle.className = "section-group-title";
                    const titleLink = document.createElement("a");
                    titleLink.href = section.url || "#";
                    titleLink.target = section.target || "_self";
                    titleLink.textContent = section.title;
                    sectionTitle.appendChild(titleLink);
                    sectionGroup.appendChild(sectionTitle);

                    // Subsections grid oluştur (3. seviye alt kategoriler)
                    if (section.subsections && section.subsections.length > 0) {
                        const sectionsGrid = document.createElement("div");
                        sectionsGrid.className = "sections-grid";

                        section.subsections.forEach((subsection, subIndex) => {
                            const sectionCard = document.createElement("a");
                            sectionCard.className = "section-card";
                            sectionCard.href = subsection.url || "#";
                            sectionCard.target = subsection.target || "_self";
                            sectionCard.style.animationDelay = `${subIndex * 0.05}s`;

                            let iconHtml = '';
                            // Ikon alanından çek (resim yolu veya Font Awesome ikon sınıfı olabilir)
                            const gecerliIkon = subsection.ikon && subsection.ikon.trim() !== "" && !subsection.ikon.includes("resimyok.png");
                            
                            if (gecerliIkon) {
                                // Eğer resim uzantısı varsa veya URL ise resim olarak göster
                                if (subsection.ikon.includes(".png") || subsection.ikon.includes(".jpg") || subsection.ikon.includes(".jpeg") || 
                                    subsection.ikon.includes(".gif") || subsection.ikon.includes(".webp") || 
                                    subsection.ikon.startsWith("/") || subsection.ikon.startsWith("http")) {
                                    iconHtml = `<img src="${subsection.ikon}" alt="${subsection.title}">`;
                                } else {
                                    // Font Awesome ikon sınıfı olarak göster
                                    iconHtml = `<i class="${subsection.ikon}"></i>`;
                                }
                            } else {
                                // İkon yoksa kategori adının ilk harfini göster
                                const ilkHarf = subsection.title && subsection.title.trim() !== "" ? subsection.title.substring(0, 1).toUpperCase() : "?";
                                iconHtml = `<span class="section-initial">${ilkHarf}</span>`;
                            }

                            sectionCard.innerHTML = `
                                <div class="section-icon">
                                    ${iconHtml}
                                </div>
                                <h4>${subsection.title}</h4>
                            `;

                            sectionsGrid.appendChild(sectionCard);
                        });

                        sectionGroup.appendChild(sectionsGrid);
                    }

                    sectionsContainer.appendChild(sectionGroup);
                });
            } else {
                // Alt kategori yoksa sadece kategori linkini göster
                sectionsContainer.innerHTML = `
                    <div style="text-align: center; padding: 40px;">
                        <a href="${category.url}" target="${category.target}" style="display: inline-block; margin-top: 20px; padding: 10px 20px; background: #667eea; color: white; text-decoration: none; border-radius: 5px;">
                            ${category.title}
                        </a>
                    </div>
                `;
            }
        }
    }

    // DOMContentLoaded veya jQuery ready'de çalıştır
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initAccordionMenu);
    } else {
        // DOM zaten yüklendi
        if (typeof jQuery !== 'undefined') {
            jQuery(document).ready(initAccordionMenu);
        } else {
            initAccordionMenu();
        }
    }
})();