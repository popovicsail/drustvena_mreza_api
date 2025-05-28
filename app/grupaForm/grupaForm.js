let grupaMember = []
let grupaNonMember = []

let urlParams = new URLSearchParams(window.location.search)
let grupaId = urlParams.get('id')

function Initialize() {
  getAllMembers()
  getAllNonMembers()
}


function getAllMembers() {
  fetch(`http://localhost:46211/api/grupa/${grupaId}/korisnik/members`)
    .then(response => {
      if (!response.ok) {
        throw new Error('Request failed. Status: ' + response.status)
      }
      return response.json()
    })
    .then(data => memberSort(data))
    .catch(error => {
      console.error('Error:', error.message)
      let table = document.querySelector('table')
      if (table) {
        table.style.display = 'none'
      }
      alert('ERROR: Došlo je do greške pri učitavanju usera. Probajte ponovo.')
    })
}

function memberSort(data) {
  data.forEach(korisnik => {
    grupaMember.push(korisnik)
  })
  addToTable("grupa-member-table", data)
}

function getAllNonMembers() {
  fetch(`http://localhost:46211/api/grupa/${grupaId}/korisnik/nonmembers`)
    .then(response => {
      if (!response.ok) {
        throw new Error('Request failed. Status: ' + response.status)
      }
      return response.json()
    })
    .then(data => nonMemberSort(data))
    .catch(error => {
      console.error('Error:', error.message)
      let table = document.querySelector('table')
      if (table) {
        table.style.display = 'none'
      }
      alert('ERROR: Došlo je do greške pri učitavanju usera. Probajte ponovo.')
    })
}

function nonMemberSort(data) {
  data.forEach(korisnik => {
    grupaNonMember.push(korisnik)
})
  addToTable("grupa-nonmember-table", data)
}

function addToTable(elementName, data) {
 let table = document.getElementById(elementName)
 table.innerHTML = ''

 data.forEach(korisnik => {
  let tr = document.createElement('tr')

  let korisnikId = korisnik.id

  let korisnickoIme = document.createElement('td')
  korisnickoIme.textContent = korisnik.korisnickoIme

  let name = document.createElement('td')
  name.textContent = korisnik.ime

  let tableButtonTd = document.createElement("td")
  let tableButton = document.createElement('button')
  tableButtonTd.appendChild(tableButton)

  if (elementName == "grupa-member-table") {
    tableButton.textContent = "remove"
    tableButton.addEventListener("click", function() {
    removeMemberFromGroup(korisnikId, grupaId)
    window.location.reload();
    })
  } 
  else {
    tableButton.textContent = "add"
    tableButton.addEventListener("click", function() {
    addMemberToGroup(korisnikId, grupaId)
    window.location.reload();
  })
  }

  tr.appendChild(korisnickoIme)
  tr.appendChild(name)
  tr.appendChild(tableButtonTd)
  table.appendChild(tr)
  
 })
}

function removeMemberFromGroup(korisnikId, groupId) {

  let requestBody = {
    KorisnikId: korisnikId,
    GrupaId: groupId
  }

  fetch(`http://localhost:46211/api/korisnik/${korisnikId}/removeClanstvo`, {
  method: 'POST',
    headers: {
    'Content-Type': 'application/json'
  },
  body: JSON.stringify(requestBody)
})
.then(response => {
  if (!response.ok) 
    throw new Error('Greška pri uklanjanju korisnika iz grupe')
  return response.json()
})
.then(data => console.log('Uspešno uklonjen:', data))
.catch(error => console.error(error))
}


function addMemberToGroup(korisnikId, groupId) {

  let requestBody = {
    "KorisnikId": korisnikId,
    "GrupaId": groupId
  }

  fetch(`http://localhost:46211/api/korisnik/${korisnikId}/addClanstvo`, {
  method: 'POST',
    headers: {
    'Content-Type': 'application/json'
  },
  body: JSON.stringify(requestBody)
})
.then(response => {
  if (!response.ok) 
    throw new Error('Greška pri uklanjanju korisnika iz grupe')
  return response.json()
})
.then(data => console.log('Uspešno uklonjen:', data))
.catch(error => console.error(error))
}

document.addEventListener("DOMContentLoaded", function() {
  Initialize()
})