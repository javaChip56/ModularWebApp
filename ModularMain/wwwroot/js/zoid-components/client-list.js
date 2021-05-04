const ClientList = zoid.create({
    tag: 'zoid-client-list',
    url: 'http://localhost/modularrazorweb/clientlist/show',
    dimensions: {
        width: '100%',
        height: '100%'
    }
});

ClientList().render('#container-client-list');