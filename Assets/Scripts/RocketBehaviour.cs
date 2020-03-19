using Photon.Pun;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(PhotonView))]
public class RocketBehaviour : MonoBehaviour
{
    public float damage = 20;

    [SerializeField] private Quaternion rot;
    [SerializeField] private PhotonView photonView;
    [SerializeField] private Photon.Realtime.Player owner;

    private Vector3 _rocketDirection;
    private float _rocketRadius = 5;

    private void Start()
    {
        _rocketDirection = transform.forward * 6;
        photonView = GetComponent<PhotonView>();
        owner = photonView.Owner;
    }

    // Update is called once per frame
    private void Update()
    {

        transform.localRotation = Quaternion.LookRotation(_rocketDirection);
        transform.Translate(transform.forward / 3 * Time.deltaTime * 40, Space.World);
        _rocketDirection.y -= Time.deltaTime / 4;
        print(owner.NickName);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] _others = Physics.OverlapSphere(transform.position,_rocketRadius);
        _others = _others.Where(p => p.gameObject.GetComponentInParent<PlayerController>() != null).Distinct().ToArray();
        foreach (Collider _one in _others)
        {
            PlayerController _playerController = _one.gameObject.GetComponentInParent<PlayerController>();
            if (_playerController)
            {
                float _distanceToOne = Vector3.Distance(transform.position, _one.transform.position);
                float _willDamage = damage - (_distanceToOne / _rocketRadius) * damage;
                _playerController.photonView.RPC(nameof(_playerController.TakeDamage), _playerController.photonView.Owner, _willDamage, owner);
                EventManager.UpdateTopListEvent.Invoke();
            }
        }
        Destroy(gameObject);
    }
}
