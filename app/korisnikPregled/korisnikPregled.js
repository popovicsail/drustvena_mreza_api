function initialize() {
  let tableButton = document.querySelector(".tableButton")
  tableButton.addEventListener("click", function() {
  window.location.href = '../korisnikForm/korisnikForm.html'
  })

  getAll()
}

function getAll() {
  fetch('http://localhost:46211/api/korisnik')
    .then(response => {
      if (!response.ok) {
        throw new Error('Request failed. Status: ' + response.status)
      }
      return response.json()
    })
    .then(data => renderData(data))
    .catch(error => {
      console.error('Error:', error.message)
      let table = document.querySelector('table')
      if (table) {
        table.style.display = 'none'
      }
      alert('ERROR: Došlo je do greške pri učitavanju usera. Probajte ponovo.')
    })
}

function renderData(data) {
  let table = document.querySelector('table tbody')
  table.innerHTML = ''

  data.forEach(korisnik => {
    let newRow = document.createElement('tr')

    let cell1 = document.createElement('td')
    cell1.textContent = korisnik.korisnickoIme
    newRow.appendChild(cell1)

    let cell2 = document.createElement('td')
    cell2.textContent = korisnik['ime']
    newRow.appendChild(cell2)

    let cell3 = document.createElement('td')
    cell3.textContent = korisnik['prezime']
    newRow.appendChild(cell3)

    let cell4 = document.createElement('td')
    cell4.textContent = korisnik['datumRodjenja']
    newRow.appendChild(cell4)

    let cell5 = document.createElement('td')
    let editButton = document.createElement('button')
    editButton.textContent = 'Edit'
    editButton.className = 'tableButton'
    editButton.addEventListener('click', function () {
      window.location.href = '../korisnikForm/korisnikForm.html?id=' + korisnik['id']
    })
    cell5.appendChild(editButton)
    newRow.appendChild(cell5)

    let cell6 = document.createElement('td')
    let deleteButton = document.createElement('button')
    deleteButton.textContent = 'Delete'
    deleteButton.className = 'tableButton'
    deleteButton.addEventListener('click', function () {
      fetch('http://localhost:46211/api/korisnik/' + korisnik['id'], { method: 'DELETE' })
        .then(response => {
          if (!response.ok) {
            const error = new Error('ERROR: Status: ' + response.status)
            error.response = response
            throw error
          }
          getAll()
        })
        .catch(error => {
          console.error('Error:', error.message)
          if (error.response && error.response.status === 404) {
            alert('ERROR: Korisnik ne postoji!')
          } else {
            alert('ERROR: Došlo je do greške pri brisanju korisnika. Probajte ponovo.')
          }
        })
    })
    cell6.appendChild(deleteButton)
    newRow.appendChild(cell6)

    table.appendChild(newRow)
  })

}

document.addEventListener('DOMContentLoaded', initialize)