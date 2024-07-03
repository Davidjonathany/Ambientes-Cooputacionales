// sidebar-toggle.js

document.getElementById("sidebarToggle").addEventListener("click", function (e) {
    e.preventDefault();
    document.getElementById("wrapper").classList.toggle("toggled");
});