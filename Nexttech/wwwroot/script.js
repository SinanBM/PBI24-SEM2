function sayHello() {
    document.getElementById("output").innerText = "Hello from JavaScript!";
}
document.querySelectorAll('.settings-link').forEach(link => {
    link.addEventListener('click', function(event) {
      console.log("You clicked on: " + link.textContent);
      // Optional: Add confirmation or animation
    });
  });