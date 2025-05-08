function sayHello() {
    document.getElementById("output").innerText = "Hello from JavaScript!";
}

document.addEventListener("click", function (e) {
    if (e.target.classList.contains("edit-btn")) {
        const id = e.target.dataset.id;
        editMaterial(id);
    }
});
