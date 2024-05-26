const uri = 'api/Films';
let films = [];

function getFilms() {
    fetch(uri)
        .then(response => response.json())
        .then(data =>
            _displayFilms(data))
        .catch(error => console.error('Unable to get films.', error));
}

function searchFilm() {
    const filmName = document.getElementById('search-name').value.trim();
    if (!filmName) {
        console.error('Film name is required');
        return;
    }

    fetch(`/api/Films/search?name=${encodeURIComponent(filmName)}`)
        .then(response => {
            console.log('Raw response:', response);
            console.log('Code response:', response.ok);
            if (!response.ok) {
                throw new Error('Film not found');
            }
            return response.json();
        })
        .then(film => {
            const filmsTable = document.getElementById('films');
            filmsTable.innerHTML = '';

            const row = filmsTable.insertRow();

            const titleCell = row.insertCell();
            titleCell.textContent = film.title;

            const yearCell = row.insertCell();
            yearCell.textContent = film.year;

            const genreCell = row.insertCell();
            genreCell.textContent = film.genreName;

            const actionsCell = row.insertCell();
            const editButton = document.createElement('button');
            editButton.textContent = 'Edit';
            editButton.onclick = () => displayEditForm(film.id);
            actionsCell.appendChild(editButton);

            const deleteButton = document.createElement('button');
            deleteButton.textContent = 'Delete';
            deleteButton.onclick = () => deleteFilm(film.id);
            actionsCell.appendChild(deleteButton);
        })
        .catch(error => console.error('Unable to search for film.', error));
}

function addFilm() {
    const addTitleTextbox = document.getElementById('add-title');
    const addYearTextbox = document.getElementById('add-year');
    const addGenreTextbox = document.getElementById('add-genre');

    const film = {
        title: addTitleTextbox.value.trim(),
        year: parseInt(addYearTextbox.value.trim(), 10), // Unix timestamp as a number 
        genreName: addGenreTextbox.value.trim()
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(film)
    })
        .then(response => response.json())
        .then(() => {
            getFilms();
            addTitleTextbox.value = '';
            addYearTextbox.value = '';
            addGenreTextbox.value = '';
        })
        .catch(error => console.error('Unable to add film.', error));
}

function deleteFilm(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getFilms())
        .catch(error => console.error('Unable to delete film.', error));
}

function displayEditForm(id) {
    console.log('Displaying edit form for film ID:', id);
    const film = films.find(film => film.id === id);
    if (film) {
        document.getElementById('edit-id').value = film.id;
        document.getElementById('edit-title').value = film.title;
        document.getElementById('edit-year').value = film.year;
        document.getElementById('edit-genre').value = film.genreName;
        document.getElementById('editFilm').style.display = 'block';
    } else {
        console.error('Film not found with ID:', id);
    }
}

function updateFilm() {
    const filmId = document.getElementById('edit-id').value;
    const film = {
        id: filmId,
        title: document.getElementById('edit-title').value.trim(),
        year: parseInt(document.getElementById('edit-year').value.trim(), 10), // Unix timestamp as a number
        genreName: document.getElementById('edit-genre').value.trim()
    };

    fetch(`${uri}/${filmId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(film)
    })
        .then(() => getFilms())
        .catch(error => console.error('Unable to update film.', error));

    closeInput();
    return false;
}

function closeInput() {
    document.getElementById('editFilm').style.display = 'none';
}

function cancelSearch() {
    document.getElementById('search-name').value = '';
    getFilms();
}

function _displayFilms(data) {
    const tBody = document.getElementById('films');
    tBody.innerHTML = '';

    const button = document.createElement('button');

    data.forEach(film => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm('${film.id}')`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteFilm('${film.id}')`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textNode = document.createTextNode(film.title);
        td1.appendChild(textNode);

        let td2 = tr.insertCell(1);
        let textNodeYear = document.createTextNode(film.year);
        td2.appendChild(textNodeYear);

        let td3 = tr.insertCell(2);
        let textNodeGenre = document.createTextNode(film.genreName);
        td3.appendChild(textNodeGenre);

        let td4 = tr.insertCell(3);
        td4.appendChild(editButton);

        let td5 = tr.insertCell(4);
        td5.appendChild(deleteButton);
    });
    films = data;
    document.getElementById('counter').innerText = `Total films: ${data.length}`;
}
getFilms();
