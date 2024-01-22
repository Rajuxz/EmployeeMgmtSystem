document.addEventListener("DOMContentLoaded", function () {
    document.addEventListener('keydown', function (e) {
        if (e.key === '/') {
            if (e.ctrlKey || e.metaKey) {
                document.getElementById("searchBox").focus();
            } else {
                return true;
            }
        }
    })
})