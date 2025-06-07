window.clickTab = (el) => {
    let e = document.getElementById(el);
    if (e == undefined || e == null)
        return;

    e.click();
};