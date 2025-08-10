window.registerOutsideClick = function (dotNetHelper, menuElement, buttonElement) {
    const menu = document.getElementById(menuElement.id);
    const button = document.getElementById(buttonElement.id);

    document.addEventListener("click", function (event) {
        if (!menu || !button) return; // safety check
        if (!menu.contains(event.target) && !button.contains(event.target)) {
            dotNetHelper.invokeMethodAsync("OnOutsideClick");
        }
    });
};
