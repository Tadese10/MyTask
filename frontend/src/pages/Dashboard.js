import React, { useState, useEffect, useRef } from 'react'
import { Link, useNavigate } from "react-router-dom"
import Swal from 'sweetalert2'
import axios from 'axios'
import Layout from "../components/Layout"
import Modal from '../components/Modal'; // Import the Modal component
import TimeRangePicker from '../components/TimeRangePicker'; // Adjust the path as necessary
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import TimePicker from 'react-time-picker';


function Dashboard() {
    const navigate = useNavigate();
    const [isListLoading, setIsListLoading] = useState(false);
    const [isTaskInProgressLoading, setIsTaskRemovingLoading] = useState(false);
    const userData = JSON.parse(localStorage.getItem('user'));
    const initialized = useRef(false);
    const [lists, setLists] = useState({});
    const [groups, setGroups] = useState({});
    const [tasks, setTasks] = useState({});
    const [selectedListId, setSelectedListId] = useState({});
    const [tasksCount, setTasksCount] = useState(0);
    const days = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
    const months = [
        'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
        'July', 'Aug', 'Sep', 'October', 'Nov', 'Dec'
    ];
    const [startDate, setStartDate] = useState(new Date());
    const [endDate, setEndDate] = useState(new Date());
    const [time, setTime] = useState('10:00');
    const [selectedDropDownListId, setSelectedDropDownListId] = useState({});
    const [selectedDropDownGroupId, setSelectedDropDownGroupId] = useState({});
    const [selectedDropDownPriority, setSelectedDropDownPriority] = useState({});
    const [selectedDropDownListType, setSelectedDropDownListType] = useState({});
    const [editing, setEditing] = useState(false);
    const [newTask, setNewTask] = useState(false);
    const [taskTitle, setTaskTitle] = useState('');
    const [taskDescription, setTaskDescription] = useState('');


    const fetchList = async () => {
        setIsListLoading(true);
        axios.defaults.headers.common = { 'Authorization': `bearer ${userData.token}` }
        const res = await axios.get(`http://localhost:5000/Lists`);
        setLists(JSON.parse(JSON.stringify(res.data.value)));
        setIsListLoading(false);
    };

    const deleteTask = async (event, taskId) => {
        event.preventDefault();
        console.log(tasks);
        tasks.filter(d => d.id == taskId).map(p => p.isTaskInProgressLoading = true);
        console.log(tasks);
        setTasks(tasks);

        axios.defaults.headers.common = { 'Authorization': `bearer ${userData.token}` }
        const res = await axios.delete(`http://localhost:5000/Tasks/${taskId}`);

        tasks.filter(d => d.id == taskId).map(p => p.isTaskInProgressLoading = false);
        setTasks(tasks);
        getListTasks(event, selectedListId);

    };

    const formatDate = (date) => {
        const dayName = days[date.getDay()];  // e.g., 'Wed'
        const dayNumber = date.getDate();     // e.g., 26
        const monthName = months[date.getMonth()];  // e.g., 'July'
        const year = date.getFullYear();      // e.g., 2023

        return `${dayName} ${dayNumber} ${monthName} ${year}`;
    };

    const [isGroupModalOpen, setIsGroupModalOpen] = useState(false);

    const [isListModalOpen, setIsListModalOpen] = useState(false);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [openedTask, setOpenedTask] = useState({});

    const openListModal = () => {
        setIsListModalOpen(true);
    };

    const openGroupModal = () => {
        setIsGroupModalOpen(true);
    };
    // Function to open the modal
    const openModal = (event, task, newTask = false, editing = false) => {
        event.preventDefault();
        setOpenedTask(task);
        setIsModalOpen(true);
        setSelectedDropDownListId(task?.listId);
        setSelectedDropDownGroupId(task?.groupId);
        setSelectedDropDownPriority(task?.priority);
        setEditing(editing);
        setStartDate(task?.startDate);
        setEndDate(task?.endDate);
        setTaskTitle(task?.title);
        setTaskDescription(task?.description);
        setNewTask(newTask);
    };

    // Function to close the modal
    const closeModal = () => {
        setIsModalOpen(false);
        setIsListModalOpen(false);
        setIsGroupModalOpen(false);
    };

    const ParseDateAndTime = ({ dateString }) => {
        const date = new Date(dateString); // Parse the date string

        // Extract time components
        const hours = date.getHours();
        const minutes = date.getMinutes();
        const seconds = date.getSeconds();

        // Format time as HH:MM:SS
        const formattedTime = `${hours?.toString().padStart(2, '0')}:${minutes
            .toString()
            .padStart(2, '0')}:${seconds?.toString().padStart(2, '0')}`;

        return (
            <div>
                <h2>Parsed Time</h2>
                <p>{formattedTime}</p>
            </div>
        );
    };

    const getTimeOfDay = () => {
        const currentHour = new Date().getHours(); // Get the current hour (0-23)

        if (currentHour >= 1 && currentHour < 12) {
            return 'morning';
        } else if (currentHour >= 12 && currentHour < 17) {
            return 'afternoon';
        } else {
            return 'evening';
        }
    };

    function getCurrentTime(dateString) {
        const date = new Date(dateString);
        let hours = date.getHours();
        const minutes = date.getMinutes();
        const ampm = hours >= 12 ? 'PM' : 'AM';

        // Convert 24-hour format to 12-hour format
        hours = hours % 12;
        hours = hours ? hours : 12; // If hours is 0, set it to 12
        const minutesStr = minutes < 10 ? '0' + minutes : minutes; // Ensure two-digit minutes

        return `${hours}:${minutesStr} ${ampm}`;
    }

    const getListTasks = async (event, list) => {
        event.preventDefault();
        setSelectedListId(list);
        axios.defaults.headers.common = { 'Authorization': `bearer ${userData.token}` }
        const res = await axios.get(`http://localhost:5000/Lists/${list.id}/Tasks`);
        const data = JSON.parse(JSON.stringify(res.data.value));
        data.map(p => p.isTaskInProgressLoading = false);
        console.log(data);
        setTasks(data);
    };


    const fetchGroups = async () => {
        axios.defaults.headers.common = { 'Authorization': `bearer ${userData.token}` }
        const res = await axios.get(`http://localhost:5000/Groups/`);
        const data = JSON.parse(JSON.stringify(res.data));
        console.log(data);
        setGroups(data);
    };

    useEffect(() => {

        if (localStorage.getItem('user') == null) {
            navigate("/");
        }
        else {

            fetchList();

            fetchGroups();

        }

    }, []);

    const AddListTask = async (event) => {
        event.preventDefault();

        if (selectedDropDownListType === "") {
            Swal.fire({
                icon: 'error',
                title: 'Invalid input taskTitle!',
                showConfirmButton: true,
                timer: 1500
            });
            return;
        }
        if (taskDescription === "") {
            Swal.fire({
                icon: 'error',
                title: 'Invalid input Description!',
                showConfirmButton: true,
                timer: 1500
            });
            return;
        }

        axios.defaults.headers.common = { 'Authorization': `bearer ${userData.token}` };
        const instance = axios.create({
            baseURL: 'http://localhost:5000/',
        });

        instance.post('/Lists/', {
            description: taskDescription,
            listType: selectedDropDownListType,
            userId: userData.id
        })
            .then(function (response) {
                // localStorage.setItem("user", JSON.stringify(response.data));
                Swal.fire({
                    icon: 'success',
                    title: 'List created successfully!',
                    showConfirmButton: false,
                    timer: 1500
                });
                fetchList();
                fetchGroups();
                closeModal();
                setTasks({});
            })
            .catch(function (error) {
                let message = '';
                console.log(Object.values(error.response.data.errors).map(item => message += item.description + ','));
                Swal.fire({
                    icon: 'error',
                    title: message ?? 'An Error Occured!',
                    showConfirmButton: false,
                    timer: 6000
                });
            });
    };

    const AddGroupTask = async (event) => {
        event.preventDefault();

        if (selectedDropDownListType === "") {
            Swal.fire({
                icon: 'error',
                title: 'Invalid input taskTitle!',
                showConfirmButton: true,
                timer: 1500
            });
            return;
        }
        if (taskDescription === "") {
            Swal.fire({
                icon: 'error',
                title: 'Invalid input Description!',
                showConfirmButton: true,
                timer: 1500
            });
            return;
        }

        axios.defaults.headers.common = { 'Authorization': `bearer ${userData.token}` };
        const instance = axios.create({
            baseURL: 'http://localhost:5000/',
        });

        instance.post('/Groups/', {
            name: taskDescription,
            groupType: selectedDropDownListType,
            userId: userData.id
        })
            .then(function (response) {
                // localStorage.setItem("user", JSON.stringify(response.data));
                Swal.fire({
                    icon: 'success',
                    title: 'Group created successfully!',
                    showConfirmButton: false,
                    timer: 1500
                });
                fetchList();
                fetchGroups();
                closeModal();
                setTasks({});
            })
            .catch(function (error) {
                let message = '';
                console.log(Object.values(error.response.data.errors).map(item => message += item.description + ','));
                Swal.fire({
                    icon: 'error',
                    title: message ?? 'An Error Occured!',
                    showConfirmButton: false,
                    timer: 6000
                });
            });
    };


    const AddOrUpdateTask = async (event) => {
        event.preventDefault();

        axios.defaults.headers.common = { 'Authorization': `bearer ${userData.token}` };
        const instance = axios.create({
            baseURL: 'http://localhost:5000/',
        });

        if (taskTitle === "") {
            Swal.fire({
                icon: 'error',
                title: 'Invalid input taskTitle!',
                showConfirmButton: true,
                timer: 1500
            });
            return;
        }
        if (taskDescription === "") {
            Swal.fire({
                icon: 'error',
                title: 'Invalid input Description!',
                showConfirmButton: true,
                timer: 1500
            });
            return;
        }
        if (startDate === null || startDate === "" || startDate === undefined) {
            Swal.fire({
                icon: 'error',
                title: 'Invalid input startDate!',
                showConfirmButton: true,
                timer: 1500
            });
            return;
        }
        if (endDate === null) {
            Swal.fire({
                icon: 'error',
                title: 'Invalid input endDate!',
                showConfirmButton: true,
                timer: 1500
            });
            return;
        }
        if (selectedDropDownPriority === null) {
            Swal.fire({
                icon: 'error',
                title: 'Invalid input Priority!',
                showConfirmButton: true,
                timer: 1500
            });
            return;
        }
        if (selectedDropDownListId === null) {
            Swal.fire({
                icon: 'error',
                title: 'Invalid input List!',
                showConfirmButton: true,
                timer: 1500
            });
            return;
        }
        if (selectedDropDownGroupId === null) {
            Swal.fire({
                icon: 'error',
                title: 'Invalid input Group!',
                showConfirmButton: true,
                timer: 1500
            });
            return;
        }

        if (editing) {

            instance.put(`/Tasks/${openedTask.id}/`, {
                title: taskTitle,
                description: taskDescription,
                startDate: startDate,
                endDate: endDate,
                priority: selectedDropDownPriority,
                listId: selectedDropDownListId,
                groupId: selectedDropDownGroupId,
                assignedUsers: [userData.id]
            })
                .then(function (response) {
                    // localStorage.setItem("user", JSON.stringify(response.data));
                    Swal.fire({
                        icon: 'success',
                        title: 'Task updated successfully!',
                        showConfirmButton: false,
                        timer: 1500
                    });
                    fetchList();
                    fetchGroups();
                    closeModal();
                    setTasks({});
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
                });
        } else {
            instance.post('/Tasks/', {
                title: taskTitle,
                description: taskDescription,
                startDate: startDate,
                endDate: endDate,
                priority: selectedDropDownPriority,
                listId: selectedDropDownListId,
                groupId: selectedDropDownGroupId,
                assignedUsers: [userData.id]
            })
                .then(function (response) {
                    // localStorage.setItem("user", JSON.stringify(response.data));
                    Swal.fire({
                        icon: 'success',
                        title: 'Task created successfully!',
                        showConfirmButton: false,
                        timer: 1500
                    });
                    fetchList();
                    fetchGroups();
                    closeModal();
                    setTasks({});
                })
                .catch(function (error) {
                    let message = '';
                    console.log(Object.values(error.response.data.errors).map(item => message += item.description + ','));
                    Swal.fire({
                        icon: 'error',
                        title: message ?? 'An Error Occured!',
                        showConfirmButton: false,
                        timer: 6000
                    });
                });

        }
    };


    const renderedLists = Object.values(lists).map((list, index) => {
        return <li class="flex justify-between items-center" key={list.id}>

            <span class="flex items-center space-x-2 btn btn-light">
                <span class="inline-block w-4 h-4 bg-[url('https://cdn-icons-png.flaticon.com/512/5807/5807625.png')]"></span>
                <button onClick={(event) => getListTasks(event, list)}> <span>{list.description}</span>        </button>

            </span>
            <span class="text-gray-500">{list.count}</span>
        </li>
    });

    const renderedGroups = Object.values(groups).map((item, index) => {
        return <div class="flex items-center space-x-2">
            {index % 2 === 0 ? <div class="w-12 h-12 bg-[url('data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAkGBwgHBgkIBwgKCgkLDRYPDQwMDRsUFRAWIB0iIiAdHx8kKDQsJCYxJx8fLT0tMTU3Ojo6Iys/RD84QzQ5OjcBCgoKDQwNGg8PGjclHyU3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3N//AABEIAJQAmAMBEQACEQEDEQH/xAAcAAABBQEBAQAAAAAAAAAAAAAAAQQFBgcDAgj/xAA6EAABAwMCBAMGBAQGAwAAAAABAgMEAAUREiEGEzFBIlFhBxRxgZGhIzJCsTPB0fAVJENSouFUk7L/xAAaAQEAAwEBAQAAAAAAAAAAAAAAAwQFAgEG/8QALxEAAgIBBAAEBAUFAQAAAAAAAAECAxEEEiExBRNBURQiMnEjUmGBsRWRocHwQv/aAAwDAQACEQMRAD8A3GgCgCgCgCgCgCgCgEz6UAZ9KAWgCgCgCgCgCgCgCgCgCgCgCgCgCgCgCgCgMI9pF/vl74rm2u0vS24kAhstx3S2FKx4lKOR3OAM9qinPBapqclwslXtyeKLAh2VAdmQhuXOU8kg98lOSD8cVyrF6Mllp5Yy0fRfC09+6cNWm4SgA/KhtPOaRgalIBOB86nKDJSgCgCgEyKAMigFoAoAoAoAoAoAoAoAoAoDk/Iajp1POJQPU16k30eNpdmPcRxOTxPdJEFR5cqQhxWEjJ8KQo79sgmqt3E8M1dJl1KSGclLrjpSgnlFpQVkAjOennk5+xqBYRceW8ehrtgkxBaoMZlYTy47aAhWxGEgYrR2yxnB885Lc1nklBXh6LQEdxBdGrLZpdzkJKm4zRXpHVR7JHqTgfOvG8I6jHc0jFp/tS4nmu/5NbERvSfw2GdZx3JKgf2FQO2XoXFp4ImOD/ajcpV5g2+9pjOMyHEs85popXrUQEk74xkjOB3r2NjbwzizTxSbibCOlTlQWgCgCgCgITiq8uWa2h9hCVPOLDbesEpBwTk49BUN1jrjks6WhXTw+uyqN8e3IMBKo0ZTufznIBHwB6/Oqvxc8dGg/Dq285eCUsfHCJchEe4spYUs6UuoPgz653FS16pSeJFe/wAPcFug8lxFWzOA0BWr6vVcFDslIH86t0r5Spc/mIa7Q2pkBsNDEtCydRGyk+RPy2qDU6eVvKLei1kdPxLpjSwWtDM5D1zR+CjJDYwrUfX07/Sq9WkszmRd1HiNbhtr9SVUfGdJ7+HFaa6MN9luhLLkVpZ3KkAk/KqMlhsux6R3rw6KT7XYb83hItxg+p0SWtLbYGFknGFZ6DfPxAqO36SfT8zwVXh22s2oKiCO0F8vU46pWXF5PQjTjT1A37dKoTeeTWgkvlOEy1NSWVMx4ERkxpbfLdZVpWgBSSTkpGMJOds/auot7kcWLMJYRsyRgAZJ+NaJii0AUAUAUBC8YMpc4cm6m0rKUak5GcHPUeuM1Des1ssaR4ujyZ3b4RafSmUhJLqTpSrfpjPb186zlF9m5KazhA/bS864WMspRjGoEBWR1B/veko45PI254NHs11YetsQvuhLymk6grbfArXrhNwUsHzt0oRtlFPpktnIyNxQ8KlxE4WZchzlrWQAQlIyT4RVqEttecZKzhvt25xkh411YdUUPAsLHUObD61HXqoSeJcMnt8Ptgk4/Mv0HHvsT/yWf/YKl86v8yIPhb/yP+x6afaeJU0sLCdiU7iuo2Rl9LOLKZ143rGSQuPGlm4chxmZzrjkktAhhhIUsDsTkgD5mq003JliDSig4d9oNkv0oRGveIkhRw23LSlPM+BSSM+mc1y4tHSkmOr/AMRW2MHYTqFSXMYU2kDA+JP/AHVK/U1wzF8sv6bSW2YmuEVtCi6wh0IKEL3Tn4+dUk88mp6teqBUxNucZecjIfSScIWdtu9eqzy2pYycyq82LgngtFi4mi3V73blqZf05CVEEK+Bq7Tqo2vb0zL1GjnQs9onqtFQKAKAbTJjMRILqtz0SOprqMXLo5lNR7ICfc35iFtABDKwUlI6qB65NWFRHGJcld3yynEhC0WANagpGcDGxx61lajTOp57TN3SaxXrb1IQNNvqU2AoNkdAftmvNNSrbMPo91l8qKsrtj3YdNu1bh832WmzKK7cyVHJ3H3NUrfrZcq+kh+O2JrlrSbPykT1uJSl10kJSncqyQCegx06kVHO7y48vglhXvlwsv8AUptssFyUtBu78RKSTzPddaiPUasd+tZ7spz6mtGy/HKRy46t54ftMa4Wx73lpT3Ke5iPyZBIO2MdCN/MedWa6q58pla3W2w42pHT2Uvv3ty5KmBAYjpQQlvIyo567nsO1WYR8pYiUrbXqOZrorVijIvV0mXG7MuO8x3CU6VadRPQ47AAD6eVR6ixpYi+SXSVRk8zXHR44qsnu0xpy0R30jTlaWMkoOdlJxv8+xHrXmnu3L52dazTbZfhom0ynZoEmRkvubuZGPF327b52rGuTVksm7pmnTHHsSNuuz8FpTKG23Wic6HE5APmN68jY4rB5bpo2PdnDG0yW9Nf5z5BVjAAGAkeQFcyk5PLJK6o1x2xHfDSyi/QiNvxdP1GKl07xbEi1azRI1Ots+cCgENAVy/x3m3lSSlS2zjdP6fQ1J8QqodHMdM7rMZwN4kNEyIt1l/8ZAyWtO9e16qNnSOb9HOn6mNFJBGkjbyNWZxU47ZdFWucq5bo8M8oQlGyRvXFVEKvpRLdqLLsb30dENlwq0/pSVn0Arqc1CO5kcISnJRiWSxnNrZPnq/+jVNzVj3R6Lnlyq+SXYw4leUhxpIKemQD2rP1fM0jR0UU02QqZLhWAlAOOoB61V2l7YsZO8y1m/WS42xOB7wyeWtQ2Q4N0k+mas6STjJ/YoayKcUR3AFuXwna5zclSX5j7oWEtg4ACcBOT6539a8filbb4ZBHRzS7IXh9+RMYVHucBTMqKoE6miEk52UnO3Wu7tud8JZTLeknmOySw0SVxcEGG9LaRz1tNE/hoJJ32HTOM/So4x3S254J52bIOTXJB8KJdu9slvyEpSUOnQ62fzqOVKBHpkCoPEVGq1KL7ItDqbJReesnsdKrGuFASkCKYl1tC85TIW04kn1Xgj61YhBxsg/fH8lSyxTqsXtk1Gto+eCgCgEKQRgjI8qAiX7KgPCRAcVGeG4wMpPyqF0rOYvDLMdS9u2ayiN/wubzFmQG0kknIOQT8O1WKr5xWJoq3UVyean+wqbW5ndaR54qb4hexCtLL1ZIRogZjyUNJ1LU0Rnurbaq9k5TLEK41tEe9xBbuFrTGRenww+oEpYSNaz4vIfHr0qKpOMEmWbIO+1uHRXZfFlo4knCPFakpc5agoPBIC09xsTvUOoj/wCkW9NXKtOMuhVMIUlCBqQhopKQg6fynIHw9PKqmfUt7U+C1cJhh2CqSxKakJWcZbXkJ9D5H0q7RVs5fqZWss3Sxjo53xpDUtJbSE6kZOO5zWR4lXGFq2rsn0km4cjSEEOPpaeGWnRoWD3z/YqHRzUblnp8EtyezK7Q7dtzVrt7oS4XHHVpBWdsAHOBWlrY+VQ174K8LXdanjhEWjQAoNhIx2TisWW7tlzGCHmWCRHkONocQpIPh1bHHat7+m2SW6DTRFHxapcTTTPMWxSX5DbalIwpQGxya9j4ZZnM3hHk/GKsfhptl1k2cJVakRmklENY8SlbgbfXOPrU8qfp2roqV6jibk+ZInKsFUKAKAKAKAZXeVFhQnZU15DDLKdanFnAArx9HUc54M/PtUsHvBQGZxZH+tyhg/LOftUW9FryZFqTf4rlhFytLrcovpIj9QCodlZ3GD1r2VkYrLOIUysnt9uzBbjKmT570m4urcluK/FK9iD0xjtjyrrOTTUVFYSOTL62JCXmVqQtKtSVJ6ivGs8M9RKzOJrnLjchb6UJIwoto0lQ9T/So1TBPOD3LPHC9xmW29wXYL6mluymm1hPRxJWAUkdwQcenapURWQUoPKN5vcJ2Ry3GQVaQQUjris/xDTTtxKC5RR01sYZUiuJXJRIAbQQUrwMgdc7VRrhVGSy/mz/AJNFuLjz0OLg7c3UhuZtvkJBSP23q9qZNJO39iChafP4byxbNbxIfKF6RgalEbkjNVYQjqrNueEdai3y45RIcic7xepS2cW1qKkoWUjCnMnofr9BX0CaVe1GG03ZkpN2u8y5Xh9NsW4y1rVo5atJIBxkn1xmsu7VWWPanwjVo0sI845Y6tHEN1tFxYauj6n4bqgk61atPqD128q5qvnCWJdHV2ni1xwzTAc1pmaLQBQBQCHpQGW+1x+VfPd7RZ0GSmK7rl8txICF48KTkjJAJOO2R6VBdNLguaaqXMsEHa+HLOixMLmxectaQVPIJWTnukoJ8Pl96qty3YRfUYqOTt7KIy3rhfLOVK5LeHUKI/KsKKc49U9fhU8Y71gq2WeTJSRP8Q2a3yLVO96ggXpmK43HWAfESNicbEjoCexryE1BbZ9knzzmpVvMWZH3OdjncEYINWCyHTfyoC/8EcHImSrTOfecQtCky+WEjCglQUn64FRRm3ZtwQaiajU2jX50piDCelyVhDTKCtavQVO+FkyUnJ4Rj1/4tXJnyTbnHDEcOUh1ASRkbjpnrmoF8LGSsVWZe+X/AB0T/B6macZW4j7JL+ezsjiCPMt8VmGwYEuIjQoIWCH0kbnIA3BAO/nV7T2VauxwujnPPJR1mls0lSnTLrjj/Ze+AW1rtKpj7qnHH3CBqUTpSkkY+uaaiqqqeyuKWPY40s7J175ybz7nnifimFCZlQ4zylzdJRhtP8Mnvnpms+7URinFPk0aaJSab6K1CiRm1IkxU4QW1FSMElR2wdRO2N9vWqkVFGptecoaX0AMoC1J1KfygAk4Rjqc9+lc2YWDl7scmrtY0JCTqGBg+day6MV9nuvQFAFAR9+uLdqs02e4FFMeOt3Cep0pJ2+leN4Oox3PBhHs7nvm7SY6nNaHW1POnPRYUnxf8t/iKo2LKya9bw8GgvtMLSgLw4kaVaSn8qgcj44OMVF9mSYb+pcDXg6ZeU8ZyFuWF+NBkNBp195vSpOjUUqz3yTjHkR5VbqThwUdS42LKZf7pBbuMJxKcc7QQ053QrG33qacIyXKKtdkq3lHz9dW7lLklcocxxPhOAE49MVDCcEsG1j2Olg4dmXq6sQWkKRrOXHD0bQOp/vzFSKcZPgjtl5cdzNxskVhi4SERh+FGaQwj0AAGP8AjXMMO2TRn3Sl5MU/Xk98ZR1SeFrm0jr7upX03/lUlnMWQ0vFiM3g2O3NuLbUhx/UgAl0YA9R3+dZ7b7NnCPNu4djSrxGbZC0RVKdYkFTiVaNTSkggg5BBI+oqWmTjNMg1CUqZI0Tgewv8OcPs26VIRIeS4ta3EAhPiUTgZ9MVfk8vJjRWFgqlx4OnuyXnkSWFF15SsEkYBJNZktLNtvJpR1EcYOq7abWERFLDuhIIURjPqK5cPLe0t0zVkcnC68P3Ca0zJjBLg0HLedJAz1Hxr2VM5LciKd8VPay38GB9FjjNyNesIOy+qRk4H0xVzTZVaTM+/G/gnqnIQoBrcZXucVb2M47VxOW2OSSuG+SiR7c9E9vITnCPGDuMnORXEZqZLOp1vDODtshlDnIix2nS3yw4hpKSE9QMgdM9qTjmLQhNxkmR9otxL3Ofbwls4AV3UP6VBVW85ZcvuWMR9SfQkrISN6tGe+D024ptZGOmxHnQ8fJWLzG4Ys13kXC7KSXJKfDFKdeSfzKCRvv51HKNcZNssQndOChH0K9aOK7Dw6pxVujS5an3PxFOAILbY6Adc9fTNRwnCHRNbVZd9Txg0ZyRbbSwqQ84zFQ6dZKzgqJ9O5qf5YLJRxZY8dlbuXtCtqNTMWM/JSQQV55aflnf7VHK+PSLUNFZ23gh7a9Gnp94T+GSNCyfEpIBJAPTNU/0NFprntiTLqizuNvJZQ84V5ShWwOP1H7V7F4lk5lDfHbnGR2z7SVg/j2xOPND2P3FWVqPdFN+H+0iWtnEsK5tc4JcZKVlJSsZxt5ivfPicPSWR65JVSGX+U4UpWArKSRmpMRlyQpyhldHU7J28q96ORzBHU+QA/v6V0jiQ7ro4CgOUloPsLbWAUrSQQa8ksrB1GW15REWe3Oxmltv46knHfaoaq3FYZZ1FynLKOyFhYyNwRmpWmnghUk1lCkEEkde4Heucno3aurfvgjsJ5qljGpG+k1x5q3bUSuh7N7eCWZY5Y1K3UfnipksFVswZ52TxHenpbmtRfXlS9JXy0/pTgdgMCqM5ZeTYrgoJJDi72BcXlCEHZHgysBOTnzGPl9vOuEztocSFT7pBjSXuc+6yCwoHKlAg7E/IijeWdVpRWEjlLtVwhR0PzIbzLSzhKnE4ya9cZJZaPY2wk8Jnm3TVQpAcGVIOy056iuDsS4S1TJKncYT0SnyFeglrLwnNu1v9/RJjMR9SgS4VZ26nAH86kjU5R3ZK1mpjCezGWOUJasMNWt0uqUs6cJxqIx08u1QsnXPZYuHf8AHJth98DjCShSlx2VNfxkgdCc7Anof3q5Up7DO1Eq1bjH3Ia28R3S5zZaXSlqO3EedU0hA8OlBxud+pFcRslJtEtlFcIrHui18AT1XKxe8qCxlwp0rOSMAVLRnbyytrElZwWWpyoFAFAIRQFPtUpq5znE2ua242HVBSml6sAKIOR28qstwcMsrRU1PC4Ji9a47LS2FFPi0qPnUdMYttNEts5RXDKevimVbpkhEWMwpWrClvJJO3wIq8tFXJJmdPXWqTXf3HQ9oEnl4VAaK/MOECvHoF+Y9XiEvykHZS05GMRbQCWVJUhOewOU/MYH0rF1mlnpp/oz6TQ62Grh+q7JR5ttkJdJbcW2ggKHYbZGT54FVEm3tj6l1ySTlLjAWm/2gW9Ea4Q3krUorU61hWCfnn7Gtr+lS2bcpnzn9Y/Fckmh3NfiTrU9Ei3Nl6IpJJZdIQ6nG+wPX5VTt0eorW3GUXqNfpZz3vh/96Gd6mHFn3R3nt4yFAVScZLho2I2Rkspiam0nLrgbQPzK/2iiTbwj1yUVlml8GwEzrHHLUpS7YoqKEpP51BR1fDxZqxCmWMS6M23VV53Vrn3O3FVgF0mWqFH0sstKUXAAfynGceu3fzqWen3xyuEiCnV+W5buWy1x2W2GUMtJCG20hKEjoAOgqRccFdvLyyqq4cSzc74YY0uzojiW9YwhBV138ireuXTFRcl6k3xUm4xkuI8j3gW1zbRYhGuKG0SC6paktq1AZ6b/CvKobI4Z7qbY2Wbo9FiqQgCgCgPK06klPnQFb4M4Mg8JsyxGdXIflOlbjzoAVp/SkY7DJ+ZPyJYPW8jrjNcmPw1OlQWkuyYzfOQ2oEhWncjbfpmuoycXlHDgpcMrnDFoTxVwjEuF0jIiz3itQcaRpynWdOQeoxjr2qWGqti8p/t6EVmlqlw1+/qQN6sUy0LPPTqb/S4noav0auFr2vh/wDdGfqNFOqO+LzH/K+4yhSRFkFS90K2UB1x5ims03xFW1d+g0Gr+Ft3vp8MfXOe0totR16iseJXkPSs7w/QTjPzLF0a3ificJV+VU857f6EUK3UfODiHAlzl6IkZx09fCnYfOop31w4lLkmhp7ZrMYvBoPB/C0ezMuSnY7Pv8n+Isb4T2SP3PrWRfZGybcVwa+nrlXDEuxzxPwla+JIfImMpbdQDyZDSQHGifI9x02O1QrgmktywxeCrIeHbA1aufzww44Q5p051LKumT3VRvLPUsInNCdQVpGobA96A9UAUAUAUAUAUAUAUAHegEAAGBQDS629m5QHor48LgwDjdJ7EV1CThJSRzOO6Li/UyudY7lCfU05Edc0/wCo02VJPqMVsw1FU1nJiT09sHhr+wxcYdQrQ40tCv8AapBB+mKlUov1ItslxgtnC3Cjkn/NXJBba/Q0oeI+p8v3rN1mo3Ly639zT0NGx+ZZH7Z/kvcaKzGaDbDYQkeVZ8YqPRoyk5cs74ro5CgEAA6UAtAFAFAFAFAFAFAFAFAFAFAFAQt0ukSLc2YbyiH3gktp28eVadv51w+0SR6ZIzVNsw3HnVBLbKCsqPYAZJrr0OF2eLXJZlReZHXrTqIJ9R1ryLTWUdWJxlhjyujgKAKAKAKAKAKAKAKAKAKAKAKAKAKAKAZybZBlTos6RFbclRNXIdUPE3qGDj5UPMDp1tDram3EhSFgpUk9we1D0ZWOzwrFbW7fbGuVGbJKUlRO5JJ3PqaN5PEklhD+h6FAFAFAFAFAFAFAFAFAFAFAFAFAFAFAFAFAFAFAFAFAFAFAFAFAFAFAf//Z')] bg-cover"></div>
                : <div class="w-12 h-12 bg-[url('https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSLeHtBb_7k867y_dnzq50hGty9trjc3PFLnklTM1DgRAzfHNZyVUadUrsBHkqEcDAuM94&usqp=CAU')] bg-cover"></div>}<div>
                <h3 class="font-medium">{item.name}</h3>
                <span class="text-sm text-gray-500">5 People</span>
            </div>
        </div>
    });

    const renderedOptionList = Object.values(lists).map(list => {
        return (<option value={list.id}>{list.description}</option>)
    });



    const renderedOptionGroup = Object.values(groups).map(group => {
        return (<option value={group.id}>{group.name}</option>)
    });


    const renderedTasks = Object.values(tasks).map((task, index) => {
        return <div class="flex justify-between items-center bg-white p-4 rounded-lg shadow">

            <input type="checkbox" class="w-5 h-5 text-blue-600 border-gray-300 rounded focus:ring-blue-500" />
            <div class="ml-3 text-lg font-medium text-gray-700 flex-grow">{task.title}</div>
            <div class="text-gray-500">{task.isTaskInProgressLoading}{task.startDate?.toString().split('T')[1].substring(0, 5)} - {task.endDate?.toString().split('T')[1].substring(0, 5)} &nbsp;&nbsp;</div>
            <button>
                {task.isTaskInProgressLoading === true ? <p >Removing...</p> :
                    <div class="dropdown">
                        <button class="dropbtn"><div class="test"></div>
                        </button>
                        <div class="dropdown-content">
                            <a onClick={(event) => openModal(event, task, false, true)}>Edit</a>
                            <a onClick={(event) => openModal(event, task, true, false)}>View</a>
                            <a onClick={(event) => deleteTask(event, task.id)}>Delete</a>
                        </div>

                    </div>}</button>

        </div>
    });

    const axiosInstance = axios.create({
        baseURL: 'https://mock-api.binaryboxtuts.com/',
    });

    const style = {
        play: {
            button: {
                width: '28',
                height: '28',
                cursor: 'pointer',
                pointerEvents: 'none',
                outline: 'none',
                backgroundColor: 'yellow',
                border: 'solid 1px rgba(255,255,255,1)',
                borderRadius: 6
            },
        }
    };

    const Logout = () => {
        localStorage.removeItem("user");
        navigate("/");
    }

    return (
        <Layout>
            <div class="bg-gray-100 font-sans">

                <div class="min-h-screen flex">
                    {/* <!-- Sidebar --> */}

                    <div class="w-1/4 bg-white p-6 border-r border-gray-200">
                        <div>
                            <h2 class="text-lg font-semibold mb-4">Private</h2>
                            {isListLoading ? <p>Loading...</p> : null}
                            <ul class="space-y-2">
                                {renderedLists}
                            </ul>
                        </div>

                        <div class="mt-6">
                            <button onClick={openListModal} class="w-full text-left text-blue-600 font-medium">+ Create new list</button>
                        </div>

                        <div class="mt-10">
                            <h2 class="text-lg font-semibold mb-4">Group</h2>
                            <div class="space-y-4">
                                {renderedGroups}
                            </div>
                            <button onClick={openGroupModal} class="w-full mt-4 text-left text-blue-600 font-medium">+ Create new group</button>
                        </div>
                    </div>

                    {/* <!-- Main Content --> */}
                    <div class="w-3/4 p-6">
                        <header class="flex justify-between items-center mb-6">
                            <div>
                                <h1 class="text-2xl font-semibold">Good {getTimeOfDay()}, {userData?.firstName}! 👋</h1>
                                <p class="text-gray-500">Today, {formatDate(new Date())}</p>
                            </div>
                            <div>
                                <button onClick={() => Logout()} className="btn btn-outline-danger float-end"> Logout </button>

                                <button onClick={(event) => openModal(event, null, false, false)} className="btn btn-success float-end"> + Create New Task </button>

                            </div>
                        </header>

                        <div class="space-y-4">
                            {renderedTasks}
                        </div>

                    </div>
                </div>

            </div>
            <Modal isOpen={isModalOpen} onClose={closeModal}>
                {openedTask !== null ? <h2>Task Details</h2> : <h2>Create New Task </h2>}
                <p><div class="task-creation">
                    <div class="d-flex align-items-center mb-3">
                        <div class="input-group mb-3" >
                            <input type="text" class="form-control" placeholder="Create new task" onChange={e => setTaskTitle(e.target.value)} value={taskTitle} />
                            {openedTask !== null ? <span>

                                <span class="input-group-text">
                                    <small>{new Date(openedTask?.startDate).getDate()} {months[new Date(openedTask?.endDate).getMonth()]}</small>
                                </span>
                                <span class="input-group-text">
                                    <small>{getCurrentTime(openedTask?.startDate)} - {getCurrentTime(openedTask?.endDate)}</small>
                                </span>

                            </span> : null}


                        </div>
                    </div>

                    <div class="d-flex align-items-center mb-3">
                        <div class="form-control" >
                            <label>Start Date:&nbsp;</label>
                            <DatePicker className='flex right'
                                selected={new Date(startDate ?? new Date())}
                                onChange={(date) => setStartDate(date)}
                                dateFormat="Pp"
                                timeFormat="HH:mm"
                                showTimeSelect='true'
                                placeholderText="Select start date and time"
                            />
                        </div>
                    </div>

                    <div class="d-flex align-items-center mb-3">
                        <div class="form-control" >
                            <label>End Date:&nbsp;</label>
                            <DatePicker className='flex right'
                                selected={new Date(endDate ?? new Date())}
                                onChange={(date) => setEndDate(date)}
                                dateFormat="Pp"
                                timeFormat="HH:mm"
                                showTimeSelect='true'
                                placeholderText="Select start date and time"
                            />
                        </div>
                    </div>
                    <div class="input-group mb-3">
                        <select class="form-select" value={selectedDropDownListId} onChange={(event) => setSelectedDropDownListId(event.target.value)}>
                            {renderedOptionList}
                        </select>
                    </div>

                    <div class="input-group mb-3">
                        <select class="form-select" value={selectedDropDownGroupId} onChange={(event) => setSelectedDropDownGroupId(event.target.value)}>
                            {renderedOptionGroup}
                        </select>
                    </div>

                    <div class="input-group mb-3">
                        <select class="form-select" value={selectedDropDownPriority} onChange={(event) => setSelectedDropDownPriority(event.target.value)}>
                            <option value='0'>Normal</option>
                            <option value='1'>Low</option>
                            <option value='2'>Medium</option>
                            <option value='3'>High</option>
                            <option value='4'>Top</option>
                        </select>
                    </div>

                    <div class="form-group mb-3">
                        <textarea class="form-control" rows="3" placeholder="Description" onChange={e => setTaskDescription(e.target.value)}>{openedTask?.description}</textarea>
                    </div>

                    <button class="btn btn-priority" disabled={newTask && !editing} onClick={event => AddOrUpdateTask(event)}> {editing ? 'Edit task' : '+ Create new task'}</button>
                </div></p>
            </Modal>

            <Modal isOpen={isListModalOpen} onClose={closeModal}>
                <h2>Create New List </h2>
                <p><div class="task-creation">

                    <div class="input-group mb-3">
                        <select class="form-select" value={selectedDropDownListType} onChange={(event) => setSelectedDropDownListType(event.target.value)}>
                            <option value='0'>Todo</option>
                            <option value='1'>Backlog</option>
                            <option value='2'>Completed</option>
                        </select>
                    </div>

                    <div class="form-group mb-3">
                        <textarea class="form-control" rows="3" placeholder="Description" onChange={e => setTaskDescription(e.target.value)}></textarea>
                    </div>

                    <button class="btn btn-priority" onClick={event => AddListTask(event)}> + Create new List</button>
                </div></p>
            </Modal>

            <Modal isOpen={isGroupModalOpen} onClose={closeModal}>
                <h2>Create New Group </h2>
                <p><div class="task-creation">

                    <div class="form-group mb-3">
                        <input class="form-control" rows="3" placeholder="Name" onChange={e => setTaskDescription(e.target.value)}></input>
                    </div>

                    <div class="input-group mb-3">
                        <select class="form-select" value={selectedDropDownListType} onChange={(event) => setSelectedDropDownListType(event.target.value)}>
                            <option value='0'>Todo</option>
                            <option value='1'>Backlog</option>
                            <option value='2'>Completed</option>
                        </select>
                    </div>


                    <button class="btn btn-priority" onClick={event => AddGroupTask(event)}> + Create new Group</button>
                </div></p>
            </Modal>
        </Layout>
    );
}

export default Dashboard;