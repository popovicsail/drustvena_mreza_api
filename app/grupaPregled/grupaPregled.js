function getAll() {
  fetch('http://localhost:46211/api/grupa')
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
      alert('ERROR: Došlo je do greške pri učitavanju grupa. Probajte ponovo.')
    })
}

function renderData(data) {
  let table = document.querySelector('.grupa-pregled-table-body')
  table.innerHTML = ''

  data.forEach(grupa => {
    let newRow = document.createElement('tr')

    let cell1 = document.createElement('td')
    cell1.textContent = grupa.id
    newRow.appendChild(cell1)

    let cell2 = document.createElement('td')
    cell2.textContent = grupa.ime
    newRow.appendChild(cell2)

    let cell3 = document.createElement('td')
    cell3.textContent = grupa.datumOsnivanja
    newRow.appendChild(cell3)

    let cell4 = document.createElement('td')
    let editButton = document.createElement('button')
    editButton.textContent = 'Edit'
    editButton.className = 'tableButton'
    editButton.addEventListener('click', function () {
      window.location.href = '../grupaForm/grupaForm.html?id=' + grupa.id
    })
    cell4.appendChild(editButton)
    newRow.appendChild(cell4)

    let cell5 = document.createElement('td')
    let deleteButton = document.createElement('button')
    deleteButton.textContent = 'Delete'
    deleteButton.className = 'tableButton'
    deleteButton.addEventListener('click', function () {
      fetch('http://localhost:46211/api/grupa/' + grupa.id, { method: 'DELETE' })
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
    cell5.appendChild(deleteButton)
    newRow.appendChild(cell5)

    table.appendChild(newRow)
  })

}

document.addEventListener("DOMContentLoaded", function() {
    getAll()
})

