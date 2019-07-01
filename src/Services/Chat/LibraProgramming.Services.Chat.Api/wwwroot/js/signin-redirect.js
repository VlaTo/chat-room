(function (window, document) {
    window.location.href = document.querySelector("meta[http-equiv=refresh]").getAttribute("data-url");
})(window, document);