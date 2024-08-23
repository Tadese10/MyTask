import React, { useState, useEffect } from 'react'
import { Link, useNavigate } from "react-router-dom"
import Swal from 'sweetalert2'
import axios from 'axios'
import Layout from "../components/Layout"


function Registration() {
    const navigate = useNavigate();
    const options = [
        { value: "female", label: "Female" },
        { value: "male", label: "Male" }
    ];
    const isValidEmail = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/g;
    const [lastName, setLastName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('')
    const [password_confirmation, setPasswordConfirmation] = useState('')
    const [isSaving, setIsSaving] = useState(false)
    const [firstName, setFirstName] = useState('');
    const handleSave = () => {
        if (firstName === "") {
            Swal.fire({
                icon: 'error',
                title: 'Invalid input FirstName!',
                showConfirmButton: true,
                timer: 1500
            });
            return;
        }
        if (lastName === "") {
            Swal.fire({
                icon: 'error',
                title: 'Invalid input LastName!',
                showConfirmButton: true,
                timer: 1500
            });
            return;
        }
        if (email === "" || !email.match(isValidEmail)) {
            Swal.fire({
                icon: 'error',
                title: 'Invalid input Email!',
                showConfirmButton: true,
                timer: 1500
            });
            return;
        }

        if (password === "" || password_confirmation === "") {
            Swal.fire({
                icon: 'error',
                title: 'Invalid input Password!',
                showConfirmButton: true,
                timer: 1500
            });
            return;
        }

        if (password !== password_confirmation) {
            Swal.fire({
                icon: 'error',
                title: 'Password does not match!',
                showConfirmButton: true,
                timer: 1500
            });
            return;
        }

        setIsSaving(true);

        const instance = axios.create({
            baseURL: 'http://localhost:5000/',
        });

        instance.post('/users/register/', {
            firstName: firstName,
            lastName: lastName,
            email: email,
            password: password
        })
            .then(function (response) {
                // localStorage.setItem("user", JSON.stringify(response.data));
                Swal.fire({
                    icon: 'success',
                    title: 'Account created successfully!',
                    showConfirmButton: false,
                    timer: 1500
                })
                setIsSaving(false);
                setEmail('');
                setPassword('');
                setLastName('');
                setFirstName('');
                setPasswordConfirmation('');
                navigate("/");
                setIsSaving(false);
            })
            .catch(function (error) {
                let message = '';
                console.log(Object.values(error.response.data.errors).map(item => message += item.description + ','));
                Swal.fire({
                    icon: 'error',
                    title: message ?? 'An Error Occured!',
                    showConfirmButton: false,
                    timer: 6000
                })
                setIsSaving(false);
            });
    }
    return (
        <Layout>
            <div className="container">
                <div className="row">
                    <div className="col-sm-9 col-md-7 col-lg-5 mx-auto">
                        <div className="card border-0 shadow rounded-3 my-5">
                            <div className="card-body p-4 p-sm-5">
                                <h5 className="card-title text-center mb-5 fw-light fs-5">Create new account</h5>
                                <form>
                                    <div className="form-floating mb-3">
                                        <input
                                            value={firstName}
                                            onChange={(event) => { setFirstName(event.target.value) }}
                                            type="text"
                                            className="form-control"
                                            id="floatingFirstName"
                                            placeholder="Jhon"
                                        />
                                        <label htmlFor="floatingFirstName">FirstName</label>
                                    </div>
                                    <div className="form-floating mb-3">
                                        <input
                                            value={lastName}
                                            onChange={(event) => { setLastName(event.target.value) }}
                                            type="text"
                                            className="form-control"
                                            id="floatingLastName"
                                            placeholder="Joe"
                                        />
                                        <label htmlFor="floatingLastName">LastName</label>
                                    </div>
                                    <div className="form-floating mb-3">
                                        <input
                                            value={email}
                                            onChange={(event) => { setEmail(event.target.value) }}
                                            type="email"
                                            className="form-control"
                                            id="floatingemail"
                                            placeholder="name@example.com" />
                                        <label htmlFor="floatingemail">Email address</label>
                                    </div>
                                    <div className="form-floating mb-3">
                                        <input
                                            value={password}
                                            onChange={(event) => { setPassword(event.target.value) }}
                                            type="password"
                                            className="form-control"
                                            id="floatingPassword"
                                            placeholder="Password" />
                                        <label htmlFor="floatingPassword">Password</label>
                                    </div>
                                    <div className="form-floating mb-3">
                                        <input
                                            value={password_confirmation}
                                            onChange={(event) => { setPasswordConfirmation(event.target.value) }}
                                            type="password"
                                            className="form-control"
                                            id="password_confirmation"
                                            name='password_confirmation'
                                            placeholder="password_confirmation " />
                                        <label htmlFor="password_confirmation">Password Confirmation</label>
                                    </div>

                                    <div className="d-grid">
                                        <button
                                            disabled={isSaving}
                                            onClick={handleSave}
                                            className="btn btn-primary btn-login text-uppercase fw-bold"
                                            type="button">
                                            Sign Up
                                        </button>
                                    </div>
                                    <hr className="my-4"></hr>

                                    <div className="d-grid">
                                        <Link className="btn btn-outline-primary btn-login text-uppercase fw-bold" to="/">Log in</Link>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </Layout>
    );
}

export default Registration;