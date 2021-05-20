const InputDetails = zoid.create({
    tag: 'zoid-input-details',
    url: 'http://localhost:81/input-details',
    dimensions: {
        width: '100%',
        height: '100%'
    }
});

InputDetails().render('#container-input-details');