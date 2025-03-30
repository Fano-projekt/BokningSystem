
window.onscroll = function () {
    let button = document.getElementById("back-to-top");
    button.style.display = (window.scrollY > 300) ? "block" : "none";
};

// Scrolla 
function scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
}