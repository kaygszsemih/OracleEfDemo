document.addEventListener('DOMContentLoaded', function () {
    document.addEventListener('input', function (e) {
        const input = e.target;
        if (!input.classList.contains('universal-input')) return;

        let value = input.value;

        // Ortak tüm input kontrolleri
        value = value
            .replace(/''|‘|’|'/g, '`')   // tek tırnaklar → `
            .replace(/""/g, '´´')        // iki çift tırnak → ´´
            .replace(/"/g, '´´')         // çift tırnak → ´´
            .replace(/</g, '«')          // < → «
            .replace(/>/g, '»')          // > → »
            .replace(/=/g, '═')          // = → ═
            .replace(/\†/g, '\+')         // + → †
            .replace(/\s\s+/g, ' ')      // çoklu boşluk → tek boşluk
            .replace(/^\s+/, '')         // baştaki boşluklar silinir
            .replace(/\s+$/, '');        // sondaki boşluklar silinir

        // E-mail özel kontrolleri
        if (input.classList.contains('email-input')) {
            let email = value.toLowerCase().trim();

            email = email
                .replace(/\.con$/i, '.com') // .con → .com
                .replace(/ğ/g, 'g')
                .replace(/ç/g, 'c')
                .replace(/ş/g, 's')
                .replace(/ü/g, 'u')
                .replace(/ö/g, 'o')
                .replace(/ı/g, 'i')
                .replace(/İ/g, 'i')
                .replace(/\s+/g, ''); // tüm boşlukları sil

            // @ ve . kontrolü
            const atCount = (email.match(/@/g) || []).length;
            const dotCount = (email.match(/\./g) || []).length;

            if (atCount > 1 || dotCount < 1) {
                // sadece görsel uyarı örneği
                input.setCustomValidity("Lütfen doğru mail adresi yazınız.");
            } else {
                input.setCustomValidity("");
            }

            input.value = email;
        }

        if (input.classList.contains('phone-input')) {
            let raw = value.replace(/\D/g, '');
            if (raw.startsWith('0')) raw = raw.substring(1);

            let part1 = raw.substring(0, 3);
            let part2 = raw.substring(3, 6);
            let part3 = raw.substring(6, 12);

            let formatted = part1;
            if (part2) formatted += '-' + part2;
            if (part3) formatted += '-' + part3;

            input.value = formatted;
        }

        if (input.classList.contains('charcount-input')) {
            const max = parseInt(input.getAttribute('data-maxlength'), 10);

            if (max) {
                if (input.value.length > max) {
                    input.value = value.substring(0, max);
                    return;
                }
                const kalan = max - value.length;
                const counter = input.parentElement.querySelector('.char-count');

                if (counter) {
                    counter.textContent = `${max}/${kalan}`;

                    if (kalan === 0) {
                        counter.classList.remove('text-muted');
                        counter.classList.add('text-danger');
                    } else {
                        counter.classList.remove('text-danger');
                        counter.classList.add('text-muted');
                    }

                    if (kalan == max)
                        counter.textContent = '';
                }
            }
        }

        if (input.classList.contains('numeric-input')) {
            value = value.replace(/[^0-9.,]/g, '');

            const hasComma = value.includes(',');
            const hasDot = value.includes('.');

            if (hasComma && hasDot) {
                const firstSeparator = value.search(/[.,]/);
                value = value[0] + value.slice(1).replace(/[.,]/g, '');
                value = value.slice(0, firstSeparator + 1) + value.slice(firstSeparator + 1).replace(/[.,]/g, '');
            }

            value = value
                .replace(/([.,]).*?([.,])/g, '$1$2'.replace(/[.,]/g, ''))
                .replace(/([.,])(?=.*[.,])/g, '');

            value = value.replace(/^[.,]/, '');
            input.value = value;
        }
    });

    const textareas = document.querySelectorAll('.charcount-input');
    textareas.forEach(function (textarea) {
        textarea.addEventListener('paste', function (e) {
            const max = parseInt(textarea.getAttribute('data-maxlength'), 10);
            const pastemessage = textarea.getAttribute('data-pastemessage') || "Yapıştırdığınız metin çok uzun.";
            const currentText = textarea.value;
            const currentLength = currentText.length;
            const paste = (e.clipboardData || window.clipboardData).getData('text');

            const selectionStart = textarea.selectionStart;
            const selectionEnd = textarea.selectionEnd;
            const selectedTextLength = selectionEnd - selectionStart;

            const newLength = currentLength - selectedTextLength + paste.length;
            const allowedLength = max - (currentLength - selectedTextLength);

            if (newLength > max) {
                e.preventDefault();

                const pasteAllowed = paste.substring(0, allowedLength);

                textarea.value =
                    currentText.substring(0, selectionStart) +
                    pasteAllowed +
                    currentText.substring(selectionEnd);

                const newCaret = selectionStart + pasteAllowed.length;
                textarea.setSelectionRange(newCaret, newCaret);

                const kalan = max - textarea.value.length;
                const counter = textarea.parentElement.querySelector('.char-count');

                if (counter)
                    counter.textContent = `${max}/${kalan}`;

                alert(pastemessage);
            }
        });
    });

    document.addEventListener('blur', function (e) {
        const el = e.target;

        if (isTextual(el)) {
            normalizeInput(el);
        }

        if (el.classList.contains('phone-input')) {
            const raw = el.value.replace(/\D/g, '');
            const errorLabel = document.querySelector('#phoneError');
            if (raw.length > 0 && raw.length < 5) {
                el.value = '';
                if (errorLabel) errorLabel.style.display = 'block';
            } else {
                if (errorLabel) errorLabel.style.display = 'none';
            }
        }

        if (el.classList.contains('text-capitalize')) {
            el.value = el.value
                .toLocaleLowerCase()
                .split(/([\s\-'.]+)/) // ayırıcıları koru
                .map(part => /[\s\-'.]+/.test(part) ? part
                    : part.charAt(0).toLocaleUpperCase() + part.slice(1))
                .join('');
        }
        else if (el.classList.contains('text-lowercase')) {
            el.value = el.value.toLocaleLowerCase().trim();
        }
        else if (el.classList.contains('text-uppercase')) {
            el.value = el.value.toLocaleUpperCase().trim();
        }

    }, true);

    document.addEventListener('submit', function (e) {
        const form = e.target;
        form.querySelectorAll('input[type="text"], textarea').forEach(normalizeInput);
    }, true);
});

function normalizeInput(el) {
    if (!isTextual(el)) return;
    if (!el || !('value' in el)) return;
    el.value = el.value.replace(/\s\s+/g, ' ').trim();
};

function isTextual(el) {
    if (!el || el.tagName !== 'INPUT' && el.tagName !== 'TEXTAREA') return false;
    // metin olmayan input tipleri
    const nonTextTypes = new Set(['file', 'checkbox', 'radio', 'range', 'color', 'button', 'submit', 'reset', 'image']);
    return el.tagName === 'TEXTAREA' || (el.tagName === 'INPUT' && !nonTextTypes.has(el.type));
};
function html_entity_decode(str) {
    var replaceAll = function (str, find, replace) {
        return str.replace(new RegExp(find, 'g'), replace);
    }

    str = replaceAll(str, "&#xC7;", "Ç");
    str = replaceAll(str, "&#xE7;", "ç");
    str = replaceAll(str, "&#x11E;", "Ğ");
    str = replaceAll(str, "&#x11F;", "ğ");
    str = replaceAll(str, "&#x130;", "İ");
    str = replaceAll(str, "&#x131;", "ı");
    str = replaceAll(str, "&#xD6;", "Ö");
    str = replaceAll(str, "&#xF6;", "ö");
    str = replaceAll(str, "&#x15E;", "Ş");
    str = replaceAll(str, "&#x15F;", "ş");
    str = replaceAll(str, "&#xDC;", "Ü");
    str = replaceAll(str, "&#xFC;", "ü");
    return str;
}

function maskName(fullName) {
    return fullName
        .trim()
        .split(" ")
        .map(part => part.charAt(0) + "*".repeat(part.length - 1))
        .join(" ");
}