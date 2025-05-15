const buttons = document.querySelectorAll('[data-page]');
const title = document.getElementById('main-title');
const contentArea = document.getElementById('content-area');
const menuButtons = document.querySelectorAll('.menu-button');

buttons.forEach(button => {
  button.addEventListener('click', () => {
    const pageName = button.getAttribute('data-page');
    title.textContent = pageName;

    if (button.classList.contains('menu-button')) {
      menuButtons.forEach(btn => btn.classList.remove('active'));
      button.classList.add('active');
    }

    loadContent(pageName);
  });
});

function loadContent(page) {
  switch (page) {
    case 'Bruger Indstillinger':
      contentArea.innerHTML = `
        <div class="user-settings">
          <table>
            <thead>
              <tr>
                <th>ID</th>
                <th>Bruger</th>
                <th>Brugernavn</th>
                <th>E-mail</th>
                <th>Rolle</th>
              </tr>
            </thead>
            <tbody>
              <!-- Rækker kommer her -->
            </tbody>
          </table>

          <div class="user-admin">
            <h3>Administrere bruger</h3>
            <label for="search-user">Søg bruger</label>
            <input type="text" id="search-user">

            <div class="btn-row">
              <button class="btn-edit">Redigere</button>
              <button class="btn-delete">Slet</button>
            </div>
          </div>
        </div>

        <div class="user-form">
          <h3>Ny Bruger</h3>
          <label>Bruger</label><input type="text"><br>
          <label>Brugernavn</label><input type="text"><br>
          <label>Kodeord</label><input type="password"><br>
          <label>E-mail</label><input type="email"><br>
          <label>Rolle</label>
          <select>
            <option>Vælg</option>
            <option>Admin</option>
            <option>Bruger</option>
          </select>

          <div class="form-buttons">
            <button class="btn-reset">Nulstil</button>
            <button class="btn-create">Opret</button>
          </div>
        </div>
      `;
      break;

    default:
      contentArea.innerHTML = `<p>Her kommer indholdet for: ${page}</p>`;
  }
}
