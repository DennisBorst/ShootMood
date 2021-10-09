using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TagsComponent : MonoBehaviour {

    [SerializeField] private List<TagSO> tags;

    public bool ContainsAnyTag(IEnumerable<TagSO> tags) {
        return this.tags.Any(x => tags.Any(y => y == x));
    }

}