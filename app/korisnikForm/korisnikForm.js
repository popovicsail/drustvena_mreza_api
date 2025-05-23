function initializeForm() {

  let submitBtn = document.querySelector("#submitBtn")
  submitBtn.addEventListener("click", submit)

  let cancelBtn = document.querySelector("#cancelBtn")
  cancelBtn.addEventListener("click", function () {
    window.location.href = '../index.html'
  })

  get()
}

function get() {

  const urlParams = new URLSearchParams(window.location.search)
  const id = urlParams.get('id')

  if (!id) {
    return
  }

  fetch('http://localhost:46211/api/korisnik/' + id)
    .then(response => {
      if (!response.ok) {
        const error = new Error('Request failed. Status: ' + response.status)
        error.response = response
        throw error
      }
      return response.json()
    })
    .then(korisnik => {
      document.querySelector('#korisnickoIme').value = korisnik.korisnickoIme
      document.querySelector('#ime').value = korisnik.ime
      document.querySelector('#prezime').value = korisnik.prezime
      document.querySelector('#datumRodjenja').value = korisnik.datumRodjenja
    })
    .catch(error => {
      console.error('Error:', error.message)
      if (error.response && error.response.status === 404) {
        alert('ERROR: Korisnik ne postoji!')
      } else {
        alert('ERROR: Došlo je do greške pri učitavanju korisnika. Probajte ponovo!')
      }
    })
}

function submit() {
  const form = document.querySelector('#form')
  const formData = new FormData(form)

  const reqBody = {
    korisnickoIme: formData.get('korisnickoIme'),
    ime: formData.get('ime'),
    prezime: formData.get('prezime'),
    datumRodjenja: formData.get('datumRodjenja')
  }

  const korisnickoImeErrorMessage = document.querySelector('#korisnickoImeError')
  korisnickoImeErrorMessage.textContent = ''
  const imeErrorMessage = document.querySelector('#imeError')
  imeErrorMessage.textContent = ''
  const prezimeErrorMessage = document.querySelector('#prezimeError')
  prezimeErrorMessage.textContent = ''
  const datumRodjenjaErrorMessage = document.querySelector('#datumRodjenjaError')
  datumRodjenjaErrorMessage.textContent = ''

  if (reqBody.korisnickoIme.trim() === '') {
    korisnickoImeErrorMessage.textContent = 'ERROR: Korisnicko ime je obavezno polje'
    return
  }
  if (reqBody.ime.trim() === '') {
    imeErrorMessage.textContent = 'ERROR: Ime je obavezno polje'
    return
  }
    if (reqBody.prezime.trim() === '') {
    prezimeErrorMessage.textContent = 'ERROR: Prezime je obavezno polje'
    return
  }
    if (reqBody.datumRodjenja.trim() === '') {
    datumRodnjenjaErrorMessage.textContent = 'ERROR: Datum Rodjenja je obavezno polje'
    return
  }

  let method = 'POST'
  let url = 'http://localhost:46211/api/korisnik'
  const urlParams = new URLSearchParams(window.location.search)
  const id = urlParams.get('id')
  if (id) {
    method = 'PUT'
    url = 'http://localhost:46211/api/korisnik/' + id
  }

  fetch(url, {
    method: method,
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(reqBody)
  })
    .then(response => {
      if (!response.ok) {
        const error = new Error('Request failed. Status: ' + response.status)
        error.response = response
        throw error
      }
      return response.json()
    })
    .then(data => {
      window.location.href = '../index.html'
    })
    .catch(error => {
      console.error('Error:', error.message)
      if (error.response && error.response.status === 404) {
        alert('ERROR: Korisnik ne postoji!')
      }
      else if (error.response && error.response.status === 400) {
        alert('ERROR: Podaci koje ste uneli nisu validni')
      }
      else {
        alert('ERROR: Došlo je do greške pri učitavanju korisnika. Probajte ponovo!')
      }
    })
}

document.addEventListener('DOMContentLoaded', initializeForm)