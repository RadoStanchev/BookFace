//Show Image
document.getElementById('postImageUrl').addEventListener('change', e => {
    e.preventDefault();
    let el = document.getElementById('postImage');
    el.src = e.target.value;
    el.hidden = false;
});

//Creating post
let postConnection = new signalR.HubConnectionBuilder().withUrl('/postHub').build();
postConnection.start()
    .catch(function (err) {
        console.log(err.toString());
    });
console.log('Posts connected')

document.getElementById('sendPost').addEventListener('click', e => {
    e.preventDefault();
    let contentEl = document.getElementById('postContent');
    let imageUrlEl = document.getElementById('postImageUrl');
    postConnection.invoke('CreatePost', contentEl.value, imageUrlEl.value)
        .catch(function (err) {
            console.log(err.toString());
        });
    contentEl.value = "";
    imageUrlEl.value = "";
});

postConnection.on("ShowPostErrors", (errors) => {
    let section = document.getElementById("postErrors");
    let source = document.getElementById('postErrorsTemplate').innerHTML;
    let template = Handlebars.compile(source);
    let resultHTML = template({ errors });
    section.innerHTML = resultHTML;
    section.hidden = false;
    setTimeout(() => {
        section.hidden = true;
    }, 7500)
});